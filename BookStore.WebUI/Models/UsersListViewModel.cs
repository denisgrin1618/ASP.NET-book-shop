﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Models
{
    public class UsersListViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

}