﻿using FundTrack.BLL.Abstract;
using FundTrack.DAL.Abstract;
using FundTrack.DAL.Entities;
using FundTrack.Infrastructure.ViewModel.FinanceViewModels;
using FundTrack.Infrastructure.ViewModel.FinanceViewModels.DonateViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FundTrack.BLL.Concrete
{
    public class FinOpService : IFinOpService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Creates new instance of FinOpService
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public FinOpService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException"></exception>
        public IEnumerable<TargetViewModel> GetTargets()
        {
            try
            {
                return this._unitOfWork.TargetRepository.Read()
                                       .Select(item => new TargetViewModel
                                       {
                                           TargetId = item.Id,
                                           Name = item.TargetName
                                       });
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates the fin op.
        /// </summary>
        /// <param name="finOpModel">The fin op model.</param>
        /// <returns></returns>
        public FinOpViewModel CreateFinOp(FinOpViewModel finOpModel)
        {
            try
            {
                var finOp = new FinOp
                {
                    AccToId = this._unitOfWork.OrganizationAccountRepository.GetOrgAccountByName(finOpModel.OrgId, finOpModel.AccToName).Id,
                    TargetId = this._unitOfWork.TargetRepository.GetTargetByName(finOpModel.TargetName).Id,
                    Amount=decimal.Parse(finOpModel.Amount),
                    Description=finOpModel.Description,
                    FinOpDate=DateTime.Now
                };
                this._unitOfWork.FinOpRepository.Create(finOp);
                var bankImportDetail = this._unitOfWork.BankImportDetailRepository.GetById(finOpModel.BankImportId);
                bankImportDetail.IsLooked = true;
                this._unitOfWork.BankImportDetailRepository.ChangeBankImportState(bankImportDetail);
                this._unitOfWork.SaveChanges();
                return finOpModel;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException(ex.Message, ex);
            }
        }
    }
}
