using Chines_auction_project.DAL;
using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public class RaffleService : IRaffleService
    {
        private readonly IRaffleDal raffleDal;
        public RaffleService(IRaffleDal raffleDal)
        {
            this.raffleDal = raffleDal;
        }
        public async Task<List<Present>> raffle()
        {
            return await raffleDal.raffle();
        }
        public async Task<List<Present>> getAllWinners()
        {
            return await raffleDal.getAllWinners();
        }
        public async Task<Present> PresentRaffle(int presentId)
        {
           return await raffleDal.PresentRaffle(presentId);
        }
        public async Task SendEmailWithAttachment(string recipientEmail, string fileName, string filePath)
        {
             await raffleDal.SendEmailWithAttachment(recipientEmail, fileName, filePath);
        }
        public async Task<string> ExportToExcelAndSendEmail<T>(List<T> dataList, string recipientEmail)
        {
            return await raffleDal.ExportToExcelAndSendEmail(dataList, recipientEmail);
        }
        public async Task<List<GiftIncomeReport>> ReportOfIncome()
        {
            return await raffleDal.ReportOfIncome();
        }
        public async Task<List<Present>> ReportOfWinners()
        {
            return await raffleDal.ReportOfWinners();
        }
        public async Task SendEmail(string recipientEmail, string gift)
        {
            await SendEmail(recipientEmail, gift);
        }
    }
}
