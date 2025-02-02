﻿using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Infrastructure.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Infrastructure.Repository.Abstracts
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
    }
}
