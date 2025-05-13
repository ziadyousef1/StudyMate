using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyMate.Controllers;
using StudyMate.Services.Interfaces;

public class SummarizeControllerTests
{
    [Fact]
    public async Task SummarizeAsPdf_ReturnsBadRequest_WhenFileIsNull()
    {
        var summarizeServiceMock = new Mock<ISummarizeService>();
        var controller = new SummarizeController(summarizeServiceMock.Object);

        var result = await controller.SummarizeAsPdf(null);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Please upload a PDF file", badRequest.Value);
    }

    [Fact]
    public async Task SummarizeAsPdf_ReturnsBadRequest_WhenFileIsNotPdf()
    {
        var summarizeServiceMock = new Mock<ISummarizeService>();
        var controller = new SummarizeController(summarizeServiceMock.Object);

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(100);
        fileMock.Setup(f => f.ContentType).Returns("image/png");

        var result = await controller.SummarizeAsPdf(fileMock.Object);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("File must be a PDF", badRequest.Value);
    }

   
}
