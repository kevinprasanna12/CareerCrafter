﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IResumeRepository : IRepository<Resume>
    {
        Task<Resume> GetByJobSeekerIdAsync(int jobSeekerId);
    }
}
