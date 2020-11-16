﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ketchup.RabbitMQ.Configurations
{
    public class RabbitMqOption
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 5672;
        public int RetryCount { get; set; }
        public int FailCount { get; set; }
        public int MessageTTL { get; set; } = 30000;
    }
}
