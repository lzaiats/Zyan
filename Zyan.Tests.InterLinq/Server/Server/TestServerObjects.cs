﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.ServiceModel;
using InterLinq.UnitTests.Artefacts.Objects;
using Zyan.InterLinq;
using Zyan.InterLinq.Communication.Remoting;
using Zyan.InterLinq.Communication.Wcf;
using InterLinq.UnitTests.Properties;
using Zyan.Communication;
using Zyan.InterLinq.Communication;

namespace InterLinq.UnitTests.Server
{
	public class TestServerObjects : TestServer
	{
		IObjectSource ObjectSource { get; set; }

		public override string DatabaseName
		{
			get { throw new NotImplementedException(); }
		}

		public override string CreateScriptName
		{
			get { throw new NotImplementedException(); }
		}

		public override string IntegrityScriptName
		{
			get { throw new NotImplementedException(); }
		}

		public override void Start()
		{
			ObjectSource = new ObjectSource();
		}

		public override void Publish()
		{
			// Create the QueryHandler
			IQueryHandler queryHandler = new ZyanObjectQueryHandler(ObjectSource);

			#region Start the WCF server

			var wcfServer = new ServerQueryWcfHandler(queryHandler);
			var binding = ServiceHelper.GetDefaultBinding();

			string serviceUri = ServiceHelper.GetServiceUri(null, null, Artefacts.ServiceConstants.ObjectsServiceName);
			wcfServer.Start(binding, serviceUri);

			#endregion

			#region Start the Zyan server

			// change service name to avoid conflict with Remoting service
			var serviceName = Artefacts.ServiceConstants.ZyanServicePrefix + Artefacts.ServiceConstants.ObjectsServiceName;
			var protocol = ZyanConstants.GetDefaultServerProtocol(ZyanConstants.DefaultServicePort);
			var host = new ZyanComponentHost(serviceName, protocol);
			host.RegisterQueryableComponent(queryHandler);

			#endregion

			#region Start the remoting server

			var remotingServer = new ServerQueryRemotingHandlerObjects(queryHandler);
			// Register default channel for remote access
			Hashtable properties = new Hashtable();
			properties["name"] = Artefacts.ServiceConstants.ObjectsServiceName;
			properties["port"] = Artefacts.ServiceConstants.ObjectsPort;
			IChannel currentChannel = RemotingConstants.GetDefaultChannel(properties);
			ChannelServices.RegisterChannel(currentChannel, false);
			remotingServer.Start(Artefacts.ServiceConstants.ObjectsServiceName, false);

			#endregion
		}
	}
}