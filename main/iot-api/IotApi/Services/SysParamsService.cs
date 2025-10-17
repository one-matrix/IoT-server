using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IotApi.DTOs;
using IotApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IotApi.Services
{
    /// <summary>
    /// 系统服务实现（系统参数、字典类型、字典数据）
    /// </summary>
    public class SysParamsService : ISysParamsService
    {
        private readonly ApplicationDbContext _context;

        public SysParamsService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region 系统参数
        public async Task<PageResult<SysParamsDto>> GetSysParamsPageAsync(SysParamsQueryDto query)
        {
            var q = _context.SysParams.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.ParamCode))
            {
                q = q.Where(p => (p.ParamCode ?? "").Contains(query.ParamCode) || (p.Remark ?? "").Contains(query.ParamCode));
            }

            var total = await q.CountAsync();
            var list = await q
                .OrderBy(p => p.ParamCode)
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
                .Select(p => new SysParamsDto
                {
                    Id = p.Id,
                    ParamCode = p.ParamCode,
                    ParamValue = p.ParamValue,
                    ParamType = p.ParamType,
                    ValueType = p.ValueType,
                    Remark = p.Remark
                })
                .ToListAsync();

            return new PageResult<SysParamsDto>
            {
                Total = total,
                List = list,
                Page = query.Page,
                Limit = query.Limit
            };
        }

        public async Task<SysParamsDto> GetSysParamAsync(long id)
        {
            var entity = await _context.SysParams.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("系统参数不存在");
            return new SysParamsDto
            {
                Id = entity.Id,
                ParamCode = entity.ParamCode,
                ParamValue = entity.ParamValue,
                ParamType = entity.ParamType,
                ValueType = entity.ValueType,
                Remark = entity.Remark
            };
        }

        public async Task<long> AddSysParamAsync(SysParamsDto dto)
        {
            var entity = new SysParams
            {
                ParamCode = dto.ParamCode,
                ParamValue = dto.ParamValue,
                ParamType = dto.ParamType,
                ValueType = dto.ValueType,
                Remark = dto.Remark,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            _context.SysParams.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateSysParamAsync(SysParamsDto dto)
        {
            if (dto.Id == null) throw new ArgumentException("参数ID不能为空", nameof(dto.Id));
            var entity = await _context.SysParams.FindAsync(dto.Id.Value);
            if (entity == null) throw new KeyNotFoundException("系统参数不存在");
            entity.ParamCode = dto.ParamCode;
            entity.ParamValue = dto.ParamValue;
            entity.ParamType = dto.ParamType;
            entity.ValueType = dto.ValueType;
            entity.Remark = dto.Remark;
            entity.UpdateDate = DateTime.UtcNow;
            _context.SysParams.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSysParamsAsync(long[] ids)
        {
            if (ids == null || ids.Length == 0) return;
            var list = await _context.SysParams.Where(p => ids.Contains(p.Id)).ToListAsync();
            if (list.Count == 0) return;
            _context.SysParams.RemoveRange(list);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SysParamExistsAsync(long id)
        {
            return await _context.SysParams.AnyAsync(p => p.Id == id);
        }
        #endregion

        #region 字典类型
        public async Task<PageResult<SysDictTypeDto>> GetDictTypePageAsync(SysDictTypeQueryDto query)
        {
            var q = _context.SysDictTypes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.DictType))
            {
                q = q.Where(t => t.DictType.Contains(query.DictType));
            }
            if (!string.IsNullOrWhiteSpace(query.DictName))
            {
                q = q.Where(t => t.DictName.Contains(query.DictName));
            }

            var total = await q.CountAsync();
            var list = await q
                .OrderBy(t => t.Sort)
                .ThenBy(t => t.DictType)
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
                .Select(t => new SysDictTypeDto
                {
                    Id = t.Id,
                    DictType = t.DictType,
                    DictName = t.DictName,
                    Remark = t.Remark,
                    Sort = t.Sort
                })
                .ToListAsync();

            return new PageResult<SysDictTypeDto>
            {
                Total = total,
                List = list,
                Page = query.Page,
                Limit = query.Limit
            };
        }

        public async Task<SysDictTypeDto> GetDictTypeAsync(long id)
        {
            var entity = await _context.SysDictTypes.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("字典类型不存在");
            return new SysDictTypeDto
            {
                Id = entity.Id,
                DictType = entity.DictType,
                DictName = entity.DictName,
                Remark = entity.Remark,
                Sort = entity.Sort
            };
        }

        public async Task<long> AddDictTypeAsync(SysDictTypeDto dto)
        {
            var entity = new SysDictType
            {
                DictType = dto.DictType,
                DictName = dto.DictName,
                Remark = dto.Remark,
                Sort = dto.Sort,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            _context.SysDictTypes.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateDictTypeAsync(SysDictTypeDto dto)
        {
            if (dto.Id == null) throw new ArgumentException("字典类型ID不能为空", nameof(dto.Id));
            var entity = await _context.SysDictTypes.FindAsync(dto.Id.Value);
            if (entity == null) throw new KeyNotFoundException("字典类型不存在");
            entity.DictType = dto.DictType;
            entity.DictName = dto.DictName;
            entity.Remark = dto.Remark;
            entity.Sort = dto.Sort;
            entity.UpdateDate = DateTime.UtcNow;
            _context.SysDictTypes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDictTypesAsync(long[] ids)
        {
            if (ids == null || ids.Length == 0) return;
            var list = await _context.SysDictTypes.Where(t => ids.Contains(t.Id)).ToListAsync();
            if (list.Count == 0) return;
            _context.SysDictTypes.RemoveRange(list);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DictTypeExistsAsync(long id)
        {
            return await _context.SysDictTypes.AnyAsync(t => t.Id == id);
        }
        #endregion

        #region 字典数据
        public async Task<PageResult<SysDictDataDto>> GetDictDataPageAsync(SysDictDataQueryDto query)
        {
            var q = _context.SysDictData.Where(d => d.DictTypeId == query.DictTypeId).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.DictLabel))
            {
                q = q.Where(d => d.DictLabel.Contains(query.DictLabel));
            }
            if (!string.IsNullOrWhiteSpace(query.DictValue))
            {
                q = q.Where(d => (d.DictValue ?? "").Contains(query.DictValue));
            }

            var total = await q.CountAsync();
            var list = await q
                .OrderBy(d => d.Sort)
                .ThenBy(d => d.DictLabel)
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
                .Select(d => new SysDictDataDto
                {
                    Id = d.Id,
                    DictTypeId = d.DictTypeId,
                    DictLabel = d.DictLabel,
                    DictValue = d.DictValue,
                    Remark = d.Remark,
                    Sort = d.Sort
                })
                .ToListAsync();

            return new PageResult<SysDictDataDto>
            {
                Total = total,
                List = list,
                Page = query.Page,
                Limit = query.Limit
            };
        }

        public async Task<SysDictDataDto> GetDictDataAsync(long id)
        {
            var entity = await _context.SysDictData.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("字典数据不存在");
            return new SysDictDataDto
            {
                Id = entity.Id,
                DictTypeId = entity.DictTypeId,
                DictLabel = entity.DictLabel,
                DictValue = entity.DictValue,
                Remark = entity.Remark,
                Sort = entity.Sort
            };
        }

        public async Task<long> AddDictDataAsync(SysDictDataDto dto)
        {
            var entity = new SysDictData
            {
                DictTypeId = dto.DictTypeId,
                DictLabel = dto.DictLabel,
                DictValue = dto.DictValue,
                Remark = dto.Remark,
                Sort = dto.Sort,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            _context.SysDictData.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateDictDataAsync(SysDictDataDto dto)
        {
            if (dto.Id == null) throw new ArgumentException("字典数据ID不能为空", nameof(dto.Id));
            var entity = await _context.SysDictData.FindAsync(dto.Id.Value);
            if (entity == null) throw new KeyNotFoundException("字典数据不存在");
            entity.DictTypeId = dto.DictTypeId;
            entity.DictLabel = dto.DictLabel;
            entity.DictValue = dto.DictValue;
            entity.Remark = dto.Remark;
            entity.Sort = dto.Sort;
            entity.UpdateDate = DateTime.UtcNow;
            _context.SysDictData.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDictDatasAsync(long[] ids)
        {
            if (ids == null || ids.Length == 0) return;
            var list = await _context.SysDictData.Where(d => ids.Contains(d.Id)).ToListAsync();
            if (list.Count == 0) return;
            _context.SysDictData.RemoveRange(list);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SysDictDataItem>> GetDictDataByTypeAsync(string dictType)
        {
            if (string.IsNullOrWhiteSpace(dictType)) return new List<SysDictDataItem>();
            var type = await _context.SysDictTypes.FirstOrDefaultAsync(t => t.DictType == dictType);
            if (type == null) return new List<SysDictDataItem>();
            return await _context.SysDictData
                .Where(d => d.DictTypeId == type.Id)
                .OrderBy(d => d.Sort)
                .ThenBy(d => d.DictLabel)
                .Select(d => new SysDictDataItem
                {
                    DictLabel = d.DictLabel,
                    DictValue = d.DictValue
                })
                .ToListAsync();
        }

        public async Task<bool> DictDataExistsAsync(long id)
        {
            return await _context.SysDictData.AnyAsync(d => d.Id == id);
        }
        #endregion
    }
}