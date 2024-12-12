using Chines_auction_project.DAL;
using Chines_auction_project.Modells;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Reflection;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mime;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Chines_auction_project.DAL
{
    public class RaffleDal: IRaffleDal
    {
        private readonly AuctionContex auctionContex;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RaffleDal(AuctionContex auctionContex,IWebHostEnvironment webHostEnvironment)
        {
            this.auctionContex = auctionContex;
            this.webHostEnvironment = webHostEnvironment;

        }
        
        public async Task<List<Present>> raffle()
        {
            var presentsList = await auctionContex.Present.Include(p => p.Purchase).ThenInclude(t => t.Tickets).ToListAsync();

            var random = new Random();

            foreach (var present in presentsList)
            {
                var duplicateTickets = new List<Ticket>();

                foreach (var purchase in present.Purchase)
                {
                    foreach (var ticket in purchase.Tickets)
                    {
                        if (ticket.PresentId == present.Id)
                        {
                            for (int i = 0; i < ticket.Quantity; i++)
                            {
                                duplicateTickets.Add(ticket);
                            }
                        }                           
                    }
                }

                if (duplicateTickets.Count > 0)
                {
                    int randomIndex = random.Next(duplicateTickets.Count);
                    var winnerTicket = duplicateTickets[randomIndex];

                    present.WinnerId = winnerTicket.Present.WinnerId; // Assuming UserId is the winner identifier

                    // Additional logic if needed for updating winner information

                    // Save changes for the winner update
                    // auctionContex.SaveChanges();
                }
            }

            // Save changes for the updates to Present entities
            // auctionContex.SaveChanges();

            return presentsList;
        }

        public async Task SendEmail(string recipientEmail, string gift)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.office365.com");
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("37325461697@mby.co.il", "Student@264");
                client.EnableSsl = true;
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("37325461697@mby.co.il", "Shulamit & Hadassa china auction");
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = "🎉🎁 Congratulations! You've won a prize! 🎁🎉";
                string body = $@"
                <html>
                <body>
                    <h2>Congratulations!</h2>
                    <p>We are happy to inform you that you won in our Chinese lottery in the <span style=""font-size: 20px;"">{gift}</span>.</p>
                    <p>We will contact you soon with more details about how to claim your prize</p>

                    <p style='color:red;'>Thank you for participating in our auction, and we hope to see you again in future events.</p>
                    <p>Best regards,
                    Shulamit & Hadassa china auction</p>
                </body>
                </html>";


                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
        public async Task<Present> PresentRaffle(int presentId)
        {
            var present = await auctionContex.Present.FirstOrDefaultAsync(d => d.Id == presentId);

            var tickets = await auctionContex.Ticket.Where(d => d.PresentId == presentId).Include(p=>p.purchase).ToListAsync ();

            var random = new Random();

            var duplicateTickets = new List<Ticket>();

            
            foreach (var ticket in tickets)
            {
                var purchase = ticket.purchase;

                if (purchase != null && purchase.Status == true)
                {
                    for (int i = 0; i < ticket.Quantity; i++)
                    {
                        duplicateTickets.Add(ticket);
                    }
                }
            }

            if (duplicateTickets.Count <= 0)

                throw new Exception("No purchases were found for this card");
            else
            {
                int randomIndex = random.Next(duplicateTickets.Count);
                var winnerTicket = duplicateTickets[randomIndex];

                present.WinnerId = winnerTicket.purchase.UserId;
                await auctionContex.SaveChangesAsync();
                

                //if (user != null && gift != null)
                //{}
                var winner= await auctionContex.User.FirstOrDefaultAsync(d => d.Id == present.WinnerId);

                await SendEmail(winner.Email,present.Name);


            }
            return present;
        }

        public async Task<List<Present>> getAllWinners()
        {
            var winnersList = await auctionContex.Present.Include(p=> p.Winner).ToListAsync();

            return winnersList;
        }

        ////
        ///
        public async Task<List<Present>> ReportOfWinners()
        {
            try
            {
                var winners = await auctionContex.Present
                    .Include(u => u.Winner)
                    .ToListAsync();
                if (winners.Count > 0)
                {
                    await ExportToExcelAndSendEmail(winners, "37325461697@mby.co.il");
                    return winners;
                }
                throw new Exception("The draw has not been made yet");

            }
            catch (Exception ex) { throw; }
        }

        ///-----------
        public async Task<List<GiftIncomeReport>> ReportOfIncome()
        {
            try
            {
                var giftQuantities = await auctionContex.Ticket
                                    .Include(g => g.Present).Include(d=>d.Present)
                                    .GroupBy(p => p.PresentId)
                                    .Select(g => new GiftIncomeReport
                                    {
                                        PresentId = (int)g.Key,
                                        Sum = g.Sum(p => p.Quantity * p.Present.Price)
                                    })
                                    .ToListAsync();

                if (giftQuantities.Count > 0)
                {
                    await ExportToExcelAndSendEmail(giftQuantities, "37325461697@mby.co.il");
                    return giftQuantities;
                }
                throw new Exception("There were no sales proceeds yet");
            }
            catch (Exception ex) { throw; }

        }

        public async Task<string> ExportToExcelAndSendEmail<T>(List<T> dataList, string recipientEmail)
        {
            try
            {
                if (dataList == null || dataList.Count == 0)
                {
                    throw new Exception("No data to export.");
                }

                var fileName = $"export_{typeof(T).Name.ToLower()}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                var filePath = Path.Combine(webHostEnvironment.ContentRootPath, fileName); // Assuming "Temp" folder for temporary storage

                // Set EPPlus license context
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial, depending on your license

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Data Export");

                    // Assuming T has public properties that you want to export
                    var properties = typeof(T).GetProperties();

                    // Add headers
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = properties[i].Name;
                    }

                    // Add data rows
                    for (int row = 0; row < dataList.Count; row++)
                    {
                        for (int col = 0; col < properties.Length; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(dataList[row]);
                        }
                    }

                    await package.SaveAsync();
                }

                // Now send the email with the file attachment
                await SendEmailWithAttachment(recipientEmail, fileName, filePath);

                // Delete the temporary file after sending
                File.Delete(filePath);

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to export and send Excel file: {ex.Message}");
            }
        }

        public async Task SendEmailWithAttachment(string recipientEmail, string fileName, string filePath)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.office365.com"))
                {
                    client.Port = 587;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("37325461697@mby.co.il", "Student@264");
                    client.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("37325461697@mby.co.il", "shulamit & hadassa china auction");
                        mailMessage.To.Add(recipientEmail);
                        mailMessage.Subject = "Your exported file";
                        mailMessage.Body = "Please find attached your exported file.";

                        // Attach the file
                        Attachment attachment = new Attachment(filePath, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(filePath);
                        disposition.ModificationDate = File.GetLastWriteTime(filePath);
                        disposition.ReadDate = File.GetLastAccessTime(filePath);
                        mailMessage.Attachments.Add(attachment);

                        // Send the email
                        await client.SendMailAsync(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email with attachment: {ex.Message}");
            }
        }





    }

}