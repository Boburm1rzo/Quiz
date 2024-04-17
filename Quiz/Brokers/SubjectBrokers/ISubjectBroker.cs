using Quiz.Models;

namespace Quiz.Brokers.SubjectBrokers;

internal interface ISubjectBroker
{
    Task<bool> InsertSubjectAsync(string name);
    Task<bool> DeleteSubjectAsync(int subjectId);
    Task<bool> UpdateSubjectAsync(Subject subject);
    Task<List<Subject>> SelectSubjectAsync();

}
