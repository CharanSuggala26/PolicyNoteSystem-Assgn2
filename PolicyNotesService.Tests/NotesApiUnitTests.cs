using Moq;
using PolicyNotesService.Models;
using PolicyNotesService.Repositories;
using PolicyNotesService.Services; 

using Xunit;

namespace PolicyNotesService.Tests
{
    public class NotesApiUnitTests
    {
        private readonly Mock<IPolicyNoteRepository> _mock;
        private readonly PolicyNoteService _service;

        public NotesApiUnitTests()
        {
            _mock = new Mock<IPolicyNoteRepository>();
            _service = new PolicyNoteService(_mock.Object);
        }

        [Fact]
        public async Task AddNoteAsync_AndReturnNote()
        {
            var newNoteGuid = Guid.NewGuid();
            var noteToCreate = new PolicyNote
            {
                PolicyNumber = "sai", 
                Note = "charan"      
            };

            var expectedNote = new PolicyNote
            {
                Id = newNoteGuid,
                PolicyNumber = noteToCreate.PolicyNumber,
                Note = noteToCreate.Note
            };

            _mock.Setup(repo => repo.AddAsync(It.IsAny<PolicyNote>()))
                     .ReturnsAsync(expectedNote);

            var result = await _service.AddNoteAsync(noteToCreate);

            _mock.Verify(repo => repo.AddAsync(It.IsAny<PolicyNote>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(newNoteGuid, result.Id);
            Assert.Equal("sai", result.PolicyNumber);
            Assert.Equal("charan", result.Note);
        }


        [Fact]
        public async Task GetAllNotesAsync_Should_CallRepositoryAndReturnAllNotes()
        {
            var notesList = new List<PolicyNote> 
            {
                new PolicyNote { Id = Guid.NewGuid(), PolicyNumber = "Exam", Note = "Fee" },
                new PolicyNote { Id = Guid.NewGuid(), PolicyNumber = "Sem", Note = "Exam" }
            };

            _mock.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(notesList);

            var result = await _service.GetAllNotesAsync();

            _mock.Verify(repo => repo.GetAllAsync(), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.Any(n => n.PolicyNumber == "Exam"));
        }

    }
}
