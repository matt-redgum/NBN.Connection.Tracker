using NBN.Connection.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NBN.Connection.Web.Controllers.Api
{
    [RoutePrefix("api/v1/statistics")]
    public class StatisticsController : ApiController
    {

        [Route("pings")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Pings(string interval)
        {
            DateTime startDateTime = DateTime.UtcNow.AddHours(-2);
            DateTime endDateTime = DateTime.UtcNow;

            switch (interval)
            {
                case "24h":
                    startDateTime = DateTime.UtcNow.AddHours(-24);
                    break;
                case "3d":
                    startDateTime = DateTime.UtcNow.AddDays(-3);
                    break;
                case "7d":
                    startDateTime = DateTime.UtcNow.AddDays(-7);
                    break;
                default:
                    break;
            }
            using (var repo = new NBNRepository(NBN.Connection.Web.Properties.Settings.Default.DBConnectionString))
            {
                var result = repo.RetrievePings(startDateTime, endDateTime);

                return Request.CreateResponse<IEnumerable<Ping>>(HttpStatusCode.OK, result);
            }
                
        }

        [Route("disconnects")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Disconnects(string interval)
        {
            DateTime startDateTime = DateTime.UtcNow.AddHours(-2);
            DateTime endDateTime = DateTime.UtcNow;

            switch (interval)
            {
                case "24h":
                    startDateTime = DateTime.UtcNow.AddHours(-24);
                    break;
                case "3d":
                    startDateTime = DateTime.UtcNow.AddDays(-3);
                    break;
                case "7d":
                    startDateTime = DateTime.UtcNow.AddDays(-7);
                    break;
                default:
                    break;
            }
            using (var repo = new NBNRepository(NBN.Connection.Web.Properties.Settings.Default.DBConnectionString))
            {
                var result = repo.RetrieveRecentDisconnections(startDateTime, endDateTime);

                return Request.CreateResponse<IEnumerable<DisconnectionInfo>>(HttpStatusCode.OK, result);
            }

        }


        [Route("downloadtests")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DownloadTests(string interval)
        {
            DateTime startDateTime = DateTime.UtcNow.AddHours(-2);
            DateTime endDateTime = DateTime.UtcNow;

            switch (interval)
            {
                case "24h":
                    startDateTime = DateTime.UtcNow.AddHours(-24);
                    break;
                case "3d":
                    startDateTime = DateTime.UtcNow.AddDays(-3);
                    break;
                case "7d":
                    startDateTime = DateTime.UtcNow.AddDays(-7);
                    break;
                default:
                    break;
            }
            using (var repo = new NBNRepository(NBN.Connection.Web.Properties.Settings.Default.DBConnectionString))
            {
                var result = repo.RetrieveDownloadSpeeds(startDateTime, endDateTime);

                return Request.CreateResponse<IEnumerable<DownloadSpeedTest>>(HttpStatusCode.OK, result);
            }

        }

        [Route("uploadtests")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage UploadTests(string interval)
        {
            DateTime startDateTime = DateTime.UtcNow.AddHours(-2);
            DateTime endDateTime = DateTime.UtcNow;

            switch (interval)
            {
                case "24h":
                    startDateTime = DateTime.UtcNow.AddHours(-24);
                    break;
                case "3d":
                    startDateTime = DateTime.UtcNow.AddDays(-3);
                    break;
                case "7d":
                    startDateTime = DateTime.UtcNow.AddDays(-7);
                    break;
                default:
                    break;
            }
            using (var repo = new NBNRepository(NBN.Connection.Web.Properties.Settings.Default.DBConnectionString))
            {
                var result = repo.RetrieveUploadSpeeds(startDateTime, endDateTime);

                return Request.CreateResponse<IEnumerable<UploadSpeedTest>>(HttpStatusCode.OK, result);
            }

        }
    }
}