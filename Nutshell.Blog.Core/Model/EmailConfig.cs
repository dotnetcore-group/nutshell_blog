namespace Nutshell.Blog.Core.Model
{
    /// <summary>
    ///     邮件发送配置
    /// </summary>
    public class EmailConfig
    {
        /// <summary>
        ///     邮箱服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     服务器端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        ///     发件人邮箱
        /// </summary>
        public string MailFrom { get; set; }

        /// <summary>
        ///     发件人用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     发件人密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     管理员通知邮箱
        /// </summary>
        public string NotifyEmail { get; set; }
    }
}