﻿namespace Raven.Studio.Commands
{
	using System.ComponentModel.Composition;
	using Caliburn.Micro;
	using Features.Indexes;
	using Framework.Extensions;
	using Messages;
	using Plugins;

	public class EditIndex
	{
		readonly IEventAggregator events;
		readonly IServer server;

		[ImportingConstructor]
		public EditIndex(IEventAggregator events, IServer server)
		{
			this.events = events;
			this.server = server;
		}

		public void Execute(string indexName)
		{
			events.Publish(new WorkStarted());
			server.OpenSession().Advanced.AsyncDatabaseCommands
				.GetIndexAsync(indexName)
				.ContinueOnSuccess(get =>
				{
					events.Publish(
						new DatabaseScreenRequested(() => new EditIndexViewModel(get.Result)));
						events.Publish(new WorkCompleted());
					});
		}
	}
}