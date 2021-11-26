﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.Department;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.Office;
using LT.DigitalOffice.Models.Broker.Requests.Position;
using LT.DigitalOffice.Models.Broker.Requests.Project;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.TextTemplateService.Models.Dto.Configuration
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    // department

    [AutoInjectRequest(typeof(IGetDepartmentsRequest))]
    public string GetDepartmentsEndpoint { get; set; }

    // position

    [AutoInjectRequest(typeof(IGetPositionsRequest))]
    public string GetPositionsEndpoint { get; set; }

    // office

    [AutoInjectRequest(typeof(IGetOfficesRequest))]
    public string GetOfficesEndpoint { get; set; }

    // user

    [AutoInjectRequest(typeof(IGetUsersDataRequest))]
    public string GetUsersDataEndpoint { get; set; }

    [AutoInjectRequest(typeof(ICheckUsersExistence))]
    public string CheckUsersExistenceEndpoint { get; set; }

    // image

    [AutoInjectRequest(typeof(IGetImagesRequest))]
    public string GetImagesEndpoint { get; set; }

    // project

    [AutoInjectRequest(typeof(IGetProjectsRequest))]
    public string GetProjectsEndpoint { get; set; }

    // email

    [AutoInjectRequest(typeof(IGetSmtpCredentialsRequest))]
    public string GetSmtpCredentialsEndpoint { get; set; }
  }
}
