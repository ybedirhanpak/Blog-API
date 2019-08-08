﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class UserFollow
    {
        [Key]
        public Guid Id { get; set; }

        public Guid FollowerId { get; set; }
        public User Follower { get; set; }
        
        public Guid FollowedId { get; set; }
        public User Followed { get; set; }
    }
        
}