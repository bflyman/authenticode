

using System.Text;
using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Console.WriteLine("输入密码并按回车:");
var pwd=Console.ReadLine();
SmtpSendEmail(pwd);

ImapReadEmail(pwd);

///<Summary>
///使用Imap协议读取邮件内容
///</Summary>
void ImapReadEmail(string pwd)
{
    using var client = new ImapClient();
    client.Connect("outlook.office365.com", 993, true);
    client.Authenticate("HodiHHP@outlook.com", pwd);
    var inbox = client.Inbox;
    inbox.Open(FolderAccess.ReadWrite);
    var query = SearchQuery.And(SearchQuery.FromContains("lijl"), SearchQuery.NotSeen);
    var uids = inbox.Search(query);
    if (uids.Any())
    {
        Console.WriteLine("有新邮件");
        foreach (var uid in uids)
        {
            var msg = inbox.GetMessage(uid);
            Console.WriteLine($"主题{msg.Subject}");
            inbox.Store(uid, new StoreFlagsRequest(StoreAction.Add, MessageFlags.Seen) { Silent = true });
        }
        inbox.Expunge();
    }
    else
    {
        Console.WriteLine("无新邮件");
    }
}

///<Summary>
///发送一般邮件
///</Summary>
void SmtpSendEmail(string pwd)
{
    using SmtpClient client = new SmtpClient();
    client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
    client.Authenticate("HodiHHP@outlook.com", pwd);
    var msg = new MimeMessage();
    msg.From.Add(new MailboxAddress("浩迪手持机测试", "HodiHHP@outlook.com"));
    msg.To.Add(new MailboxAddress("", "lijl@hodi.cn"));
    msg.Subject = "How you doin?";
    msg.Body = new TextPart("plain")
    {
        Text = "无什么内容"
    };
    client.Send(msg);

}