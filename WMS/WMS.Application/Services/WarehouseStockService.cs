using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class WarehouseStockService : IWarehouseStockService
    {
        private readonly IWarehouseStockRepository _repository;
        private readonly IMapper _mapper;

        public WarehouseStockService(IWarehouseStockRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<bool> AddNew(WarehouseStock Entity)
        {
            return await _repository.Add(Entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<WarehouseStockDto>?> GetAll()
        {
            IEnumerable<WarehouseStock> warehouseStocks = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<WarehouseStockDto>>(warehouseStocks);
        }

        public async Task<WarehouseStockDto?> GetByID(int id)
        {
            var warehouseStock = await _repository.GetByIdAsync(id);
            return _mapper.Map<WarehouseStockDto>(warehouseStock);
        }


        public async Task<bool> Update(WarehouseStock Entity)
        {
            return await _repository.Update(Entity);
        }
        public async Task<bool> IsExistCombination(int warehouseId, int itemId)
        {
            return await _repository.IsExistCombination(warehouseId, itemId);
        }


        public async Task<bool> TransferStock(WarehouseTransferDto transferDto)
        {


            // 2. Begin Transaction
            // Note: This usually requires the Repository to expose the Transaction capability
            using var transaction = await _repository.BeginTransactionAsync();

            try
            {
                // 1. Validation Logic
                var sourceStock = await _repository.GetStockByWarehouseAndItem(transferDto.FromWarehouseID, transferDto.ItemID);

                if (sourceStock == null || sourceStock.Quantity < transferDto.Quantity)
                {
                    //return OperationResult.Failure("Insufficient_Stock", ResultCode.InvalidRequest);
                    return false;
                }

                // 3. Subtract from Source
                sourceStock.Quantity -= transferDto.Quantity;
                await _repository.Update(sourceStock);

                // 4. Add to Destination (or Create if doesn't exist)
                var destinationStock = await _repository.GetStockByWarehouseAndItem(transferDto.ToWarehouseID, transferDto.ItemID);


                if (destinationStock != null)
                {
                    destinationStock.Quantity += transferDto.Quantity;
                    await _repository.Update(destinationStock);
                }
                else
                {
                    var newStock =new  AddWarehouseStockDto(transferDto.ToWarehouseID, transferDto.ItemID, transferDto.Quantity, sourceStock.BatchNumber,
                        sourceStock.ActualCost,sourceStock.ProductionDate,sourceStock.ExpiryDate,DateTime.Now , transferDto.CreatedBy );

                   
                  

                    await _repository.Add(_mapper.Map<WarehouseStock>(newStock)); 

                }

                // 5. Commit everything
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();

                //return OperationResult.Success("Transfer_Completed");
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                //return OperationResult.Failure("Transfer_Failed_Database_Error", ResultCode.Error);
                return false;
            }
        }







    }
}
