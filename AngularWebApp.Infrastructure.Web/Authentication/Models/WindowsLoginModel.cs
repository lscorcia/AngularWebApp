﻿using System.ComponentModel.DataAnnotations;

namespace AngularWebApp.Infrastructure.Web.Authentication.Models
{
    public class WindowsLoginModel
    {
        [Required]
        public string ClientId { get; set; }
    }
}