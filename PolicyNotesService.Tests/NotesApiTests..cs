using System; 
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using PolicyNotesService.Models;
using Xunit;


namespace PolicyNotesService.Tests
{
    public class NotesApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public NotesApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Add_Note_ReturnCreated()
        {
            var note = new PolicyNote
            {
                PolicyNumber = "22072A66J8",
                Note = "Education Loan"
            };

            var response = await _client.PostAsJsonAsync("/notes", note);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var policy = await response.Content.ReadFromJsonAsync<PolicyNote>();

            Assert.NotNull(policy);
            Assert.NotEqual(Guid.Empty, policy.Id);
            Assert.Equal("Education Loan", policy.Note);
            Assert.Equal("22072A66J8", policy.PolicyNumber);
        }

        [Fact]
        public async Task Get_Success_on_GetNotes()
        {
            var response = await _client.GetAsync("/notes");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Status_upon_getbyId()
        {
            var testGuid = Guid.NewGuid();

            var note = new PolicyNote
            {
                Id = testGuid,
                PolicyNumber = "22072A66J8",
                Note = "Education Loan"
            };

            var postResponse = await _client.PostAsJsonAsync("/notes", note);
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/notes/{note.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var policy = await getResponse.Content.ReadFromJsonAsync<PolicyNote>();

            Assert.NotNull(policy);
            Assert.Equal(testGuid, policy.Id);
            Assert.Equal("Education Loan", policy.Note);
            Assert.Equal("22072A66J8", policy.PolicyNumber);
        }
    }
}