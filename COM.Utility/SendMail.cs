using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
namespace COM.Utility
{
 
    /// <summary>     
    /// 1. 功能：发送邮箱
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-31
    /// 4. 最后修改日期：2014-10-31
    /// </summary>  
    public class SendMail
    {
        private Encoding _encoding = Encoding.Default;
        /// <summary>
        /// 设置邮件正文是否为 Html 格式 
        /// </summary>
        private bool _isHtml = true;
        /// <summary>
        /// 获取发送结果，成功则为空 
        /// </summary>
        private string _result;
        private SmtpClient _smtp = new SmtpClient();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SendMail(string host, string username, string password)
        {
            this._smtp.Host = host;
            this._smtp.Port = 0x19;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                this._smtp.UseDefaultCredentials = false;
            }
            else
            {
                this._smtp.Credentials = new NetworkCredential(username, password);
            }
        }



        /// <summary>
        /// 发送邮件  
        /// </summary>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="sender">发件人显示名称</param>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <returns>bool 是否成功 </returns>
        public bool Send(string from, string sender, string to, string subject, string body)
        {
            if (Regex.IsMatch(to + "", @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(from, sender);
                message.Subject = subject;
                message.SubjectEncoding = this._encoding;
                message.Body = body;
                message.BodyEncoding = this._encoding;
                message.IsBodyHtml = this._isHtml;
                message.Priority = MailPriority.Normal;
                message.To.Add(to);
                try
                {
                    this._smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            this._result = "收件人地址不合法";
            return false;
        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="sender">发件人显示名称</param>
        /// <param name="to">收件人邮件地址数组</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <returns>bool 是否成功 </returns>
        public bool Send(string from, string sender, string[] to, string subject, string body)
        {
            if (to.Length > 0)
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(from, sender);
                message.Subject = subject;
                message.SubjectEncoding = this._encoding;
                message.Body = body;
                message.BodyEncoding = this._encoding;
                message.IsBodyHtml = this._isHtml;
                message.Priority = MailPriority.Normal;
                foreach (string str in to)
                {
                    if (Regex.IsMatch(to + "", @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
                    {
                        message.To.Add(str);
                    }
                }
                try
                {
                    this._smtp.Send(message);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            this._result = "收件人地址不合法";
            return false;
        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="sender">发件人显示名称</param>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="file">附件地址</param>
        /// <returns>bool 是否成功 </returns>
        public bool Send(string from, string sender, string to, string subject, string body, string file)
        {
            if (Regex.IsMatch(to + "", @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
            {
                MailMessage message = new MailMessage();
                Attachment objMailAttachment;
                // 创建一个附件对象
                objMailAttachment = new Attachment(file);//发送邮件的附件
                message.From = new MailAddress(from, sender);
                message.Subject = subject;
                message.SubjectEncoding = this._encoding;
                message.Body = body;
                message.BodyEncoding = this._encoding;
                message.IsBodyHtml = this._isHtml;
                message.Priority = MailPriority.Normal;
                message.Attachments.Add(objMailAttachment);
                message.To.Add(to);
                try
                {
                    this._smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            this._result = "收件人地址不合法";
            return false;
        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="sender">发件人显示名称</param>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="file">附件地址数组</param>
        /// <returns>bool 是否成功 </returns>
        public bool Send(string from, string sender, string to, string subject, string body, string[] file)
        {
            if (Regex.IsMatch(to + "", @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
            {
                MailMessage message = new MailMessage();

                // 创建一个附件对象
                foreach (var r in file)
                {
                    Attachment objMailAttachment;
                    objMailAttachment = new Attachment(r);//发送邮件的附件
                    message.Attachments.Add(objMailAttachment);
                }
                message.From = new MailAddress(from, sender);
                message.Subject = subject;
                message.SubjectEncoding = this._encoding;
                message.Body = body;
                message.BodyEncoding = this._encoding;
                message.IsBodyHtml = this._isHtml;
                message.Priority = MailPriority.Normal;

                message.To.Add(to);
                try
                {
                    this._smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            this._result = "收件人地址不合法";
            return false;
        }


        public Encoding ContentEncoding
        {
            set
            {
                this._encoding = value;
            }
        }

        public bool IsHtml
        {
            set
            {
                this._isHtml = value;
            }
        }

        public string Result
        {
            get
            {
                return this._result;
            }
        }
    }
}
