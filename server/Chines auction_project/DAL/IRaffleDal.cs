using Chines_auction_project.Modells;

namespace Chines_auction_project.DAL
{
    public interface IRaffleDal
    {
        Task<List<Present>> raffle();
        Task<Present> PresentRaffle(int presentId);
        Task<List<Present>> getAllWinners();
        Task SendEmailWithAttachment(string recipientEmail, string fileName, string filePath);
        Task<string> ExportToExcelAndSendEmail<T>(List<T> dataList, string recipientEmail);
        Task<List<GiftIncomeReport>> ReportOfIncome();
        Task<List<Present>> ReportOfWinners();
        Task SendEmail(string recipientEmail, string gift);
    }
}