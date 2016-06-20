using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Reporting.WebForms;

namespace rsReports.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        [Route("users"), HttpGet]
        public HttpResponseMessage GenerateReport()
        {
            // Generate the report data.
            var reportData = GetReportBytes();

            // Create response using the report bytes.
            // Response headers are set to return a PDF document.
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(reportData) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "MyUserReport.pdf" };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }

        private static byte[] GetReportBytes()
        {
            // Get a fake datatable with dummy data.
            var demoReportData = CreateFakeDataTable();

            // Create our local report object to build our report on.
            var localReport = new LocalReport
            {
                DisplayName = "Users",
                ReportEmbeddedResource = "rsReports.Users.rdlc",
                EnableHyperlinks = true
            };

            // Pass custom parameters to our report.
            var reportParameterCollection = new ReportParameterCollection
            {
                new ReportParameter("GeneratedBy", "Ross Steytler"),
                new ReportParameter("GeneratedDate", DateTime.Now.ToShortDateString())
            };
            localReport.SetParameters(reportParameterCollection);

            // Create a ReportDataSource object to add to the local report object.
            var reportDataSource = new ReportDataSource("UserReportDataSet", demoReportData);
            localReport.DataSources.Add(reportDataSource);

            // Render the report as PDF.
            Warning[] warnings;
            string[] streams;
            string mimeType, encoding, fileNameExtension;
            var renderedReport = localReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return renderedReport;
        }

        private static DataTable CreateFakeDataTable()
        {
            // Use the strongly typed DataTable we defined for the report.
            // A normal DataTable will work too however the strongly typed DataTable is
            // required for the report definition.
            var demoReportData = new UserReportDataSet.UserDataTable();

            var userOne = demoReportData.NewUserRow();
            userOne.UserId = 1;
            userOne.FirstName = "Ross";
            userOne.LastName = "Steytler";
            userOne.Gender = "Male";
            demoReportData.AddUserRow(userOne);

            var userTwo = demoReportData.NewUserRow();
            userTwo.UserId = 2;
            userTwo.FirstName = "Michael";
            userTwo.LastName = "Lyles";
            userTwo.Gender = "Male";
            demoReportData.AddUserRow(userTwo);

            var userThree = demoReportData.NewUserRow();
            userThree.UserId = 3;
            userThree.FirstName = "Maria";
            userThree.LastName = "Nichols";
            userThree.Gender = "Female";
            demoReportData.AddUserRow(userThree);

            var userFour = demoReportData.NewUserRow();
            userFour.UserId = 4;
            userFour.FirstName = "Joan";
            userFour.LastName = "Hage";
            userFour.Gender = "Female";
            demoReportData.AddUserRow(userFour);

            var userFive = demoReportData.NewUserRow();
            userFive.UserId = 5;
            userFive.FirstName = "Jeremy";
            userFive.LastName = "Moore";
            userFive.Gender = "Male";
            demoReportData.AddUserRow(userFive);

            return demoReportData;
        }
    }
}