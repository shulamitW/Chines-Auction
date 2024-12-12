using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public interface IRaffleService
    {
        Task<List<Present>> raffle();
        Task<List<Present>> getAllWinners();
        Task<Present> PresentRaffle(int presentId);
        Task SendEmailWithAttachment(string recipientEmail, string fileName, string filePath);
        Task<string> ExportToExcelAndSendEmail<T>(List<T> dataList, string recipientEmail);
        Task<List<GiftIncomeReport>> ReportOfIncome();
        Task<List<Present>> ReportOfWinners();
        Task SendEmail(string recipientEmail, string gift);
    }
}
