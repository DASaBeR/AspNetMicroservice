﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
	public class EmailService : IEmailService
	{
		public EmailSettings _emailSettings;
		public ILogger<EmailService> _logger;

		public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
		{
			_emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<bool> SendEmail(Email email)
		{
			var client = new SendGridClient(_emailSettings.ApiKey);

			var subject = email.Subject;
			var to = new EmailAddress(email.To);
			var emailBody = email.Body;

			var from = new EmailAddress
			{
				Email = _emailSettings.FromAddress,
				Name = _emailSettings.FromName
			};

			var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
			var response = await client.SendEmailAsync(sendGridMessage);

			_logger.LogInformation("Email sent.");

			if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return true;
			}

			_logger.LogError("Email sending failed.");
			return false;
		}
	}
}
