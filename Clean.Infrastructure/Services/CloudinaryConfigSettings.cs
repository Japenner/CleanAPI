namespace Clean.Infrastructure.Services
{
    /// <summary>
    /// Definition for Cloudinary config settings
    /// </summary>
    public class CloudinaryConfigSettings
    {
        /// <summary>
        /// Gets or sets the cloud name
        /// </summary>
        public string CloudName { get; set; }

        /// <summary>
        /// Gets or sets the API key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the API secret
        /// </summary>
        public string ApiSecret { get; set; }
    }
}
