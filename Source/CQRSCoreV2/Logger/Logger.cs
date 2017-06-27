
namespace CQRSCoreV2
{
    using System;

    using Serilog;
    using Serilog.Context;
    using Serilog.Events;
    using System.Collections.Generic;
    using CQRSCoreV2.Core;
    
    //using SeriLogger = Serilog.ILogger;
    using SeriLogger = Serilog.Core.Logger;
    
    public class Logger : Core.ILogger
    {
        private readonly SeriLogger logger;

        public Logger(SeriLogger logger)
        {
            this.logger = logger;

        }

        public void Debug(string message, params object[] args)
        {
            this.logger.Debug(message, args);
        }

        public void Info(string message, params object[] args)
        {
            this.logger.Information(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            this.logger.Warning(message, args);
        }

        public void Error(string message, params object[] args)
        {
            this.logger.Error(message, args);
            //CreateTicketInJiraAndPostMessageInSlack(message);
        }

        public void Error(Exception e, string message, params object[] args)
        {
            this.logger.Error(e, message, args);
            //CreateTicketInJiraAndPostMessageInSlack(e.Message);
        }

        public void Fatal(string message, params object[] args)
        {
            this.logger.Fatal(message, args);
            //CreateTicketInJiraAndPostMessageInSlack(message);
        }

        public void Fatal(Exception e, string message, params object[] args)
        {
            this.logger.Fatal(e, message, args);
            //CreateTicketInJiraAndPostMessageInSlack(e.Message);
        }

        public IDisposable WithProperty(string name, object value)
        {
            return LogContext.PushProperty(name, value);
        }


        const string BeginningMessage = "Beginning operation {TimedOperationId}: {TimedOperationDescription} {@Properties}";
        const string CompletedMessage = "Completed operation {TimedOperationId}: {TimedOperationDescription} in {TimedOperationElapsed} ({TimedOperationElapsedInMs} ms) {@Properties}";

        const string ExceededOperationMessage = "Operation {TimedOperationId}: {TimedOperationDescription} exceeded the limit of {WarningLimit} by completing in {TimedOperationElapsed}  ({TimedOperationElapsedInMs} ms)  {@Properties}";

        public IDisposable MeasureTime(string label, string id, TimeSpan warnWhenExceeds, params object[] propertyValues)
        {
            if (propertyValues == null || propertyValues.Length == 0)
            {
                return this.logger.BeginTimedOperation(label, id, LogEventLevel.Information, warnWhenExceeds == TimeSpan.Zero ? null : (TimeSpan?)warnWhenExceeds);
            }

            return this.logger.BeginTimedOperation(description: label, identifier: id,
                                                    level: LogEventLevel.Information,
                                                    warnIfExceeds: warnWhenExceeds == TimeSpan.Zero ? null : (TimeSpan?)warnWhenExceeds,
                                                    beginningMessage: BeginningMessage,
                                                    completedMessage: CompletedMessage,
                                                    exceededOperationMessage: ExceededOperationMessage,
                                                    propertyValues: propertyValues);
        }
        
        //public void CreateTicketInJiraAndPostMessageInSlack(string message)
        //{
        //    Issue createdIssue = null;
        //    var siteInfo = "Site URL: " + appSettings["app.url"];
        //    bool isCreateTicket = appSettings["jira.create.ticket"].ToBoolean();
        //    if (isCreateTicket)
        //    {
        //        string projectKey = appSettings["jira.projectKey"];
        //        string issueType = appSettings["jira.issueType.bug"];
        //        string componentId = appSettings["jira.componentId.bbl.production.alert"];
        //        string planningStatusId = appSettings["jira.planningStatusId.readyForDevelopment"];
        //        string assignedTeamId = appSettings["jira.assignedTeamId.qa"];
        //        string summary = appSettings["jira.summary.fatal.error"];
        //        string description = siteInfo + "\n\n" + message;

        //        List<String> labels = new List<string>();
        //        string label = appSettings["jira.label.babel.error"];
        //        labels.Add(label);

        //        JiraUser assignee = new JiraUser();
        //        assignee.Name = appSettings["jira.user"];

        //        createdIssue = jiraService.CreateIssue(projectKey, issueType, componentId, planningStatusId, assignedTeamId, summary, labels, description, assignee);
        //    }

        //    bool isPostMessage = appSettings["slack.post.message"].ToBoolean();

        //    if (isPostMessage)
        //    {
        //        string userName = appSettings["slack.userName"];
        //        string channel = appSettings["slack.channel.exception"];
        //        string summary = appSettings["slack.summary.fatal.error"];
        //        string jiraIssueLink = appSettings["jira.issue.link"] + createdIssue.Key;
        //        string slackMessage = "Issue Summary: \n" + summary + "\n" + "Issue Description: \n" + "JIRA Link: " + jiraIssueLink;
        //        slackMessage = siteInfo + "\n\n" + slackMessage;
        //        slackService.PostMessage(username: userName, text: slackMessage, channel: channel);
        //    }
        //}
    }

    public class DummyDisposable: IDisposable
    {
        public void Dispose()
        {
            
        }
    }
}