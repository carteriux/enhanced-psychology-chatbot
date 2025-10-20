using CPC.Domain.DTO.ValueObjects;

namespace CPC.Infrastructure.CrossCutting.ICommon
{
    public interface ISecurity
    {
        LocalSession DecodingJwtToken(string token);
        bool Validate(string appiKey, string dateSent);
        string CreateJwtToken(LocalSession session, string key);
    }
}
