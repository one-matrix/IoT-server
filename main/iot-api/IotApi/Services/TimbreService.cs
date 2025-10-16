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
    /// 音色服务实现
    /// </summary>
    public class TimbreService : ITimbreService
    {
        private readonly ApplicationDbContext _context;

        public TimbreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<TimbreDto>> GetPageAsync(TimbreQueryDto query)
        {
            var q = _context.AiTtsVoices.AsQueryable();

            if (string.IsNullOrWhiteSpace(query.TtsModelId))
            {
                return new PageResult<TimbreDto>
                {
                    Total = 0,
                    List = new List<TimbreDto>(),
                    Page = query.Page,
                    Limit = query.Limit
                };
            }

            q = q.Where(v => v.TtsModelId == query.TtsModelId);

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                q = q.Where(v => v.Name.Contains(query.Name));
            }

            var total = await q.CountAsync();
            var items = await q
                .OrderBy(v => v.Sort)
                .ThenBy(v => v.Name)
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
                .Select(v => MapToDto(v))
                .ToListAsync();

            return new PageResult<TimbreDto>
            {
                Total = total,
                List = items,
                Page = query.Page,
                Limit = query.Limit
            };
        }

        public async Task<TimbreDto> GetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            var entity = await _context.AiTtsVoices.FindAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<string> AddAsync(TimbreDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.TtsModelId)) throw new ArgumentException("TTS模型ID不能为空", nameof(dto.TtsModelId));
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("音色名称不能为空", nameof(dto.Name));
            if (string.IsNullOrWhiteSpace(dto.TtsVoice)) throw new ArgumentException("音色编码不能为空", nameof(dto.TtsVoice));
            if (string.IsNullOrWhiteSpace(dto.Languages)) throw new ArgumentException("语言不能为空", nameof(dto.Languages));

            var id = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid().ToString("N").Substring(0, 32) : dto.Id;

            var entity = new AiTtsVoice
            {
                Id = id,
                TtsModelId = dto.TtsModelId,
                Name = dto.Name,
                TtsVoice = dto.TtsVoice,
                Languages = dto.Languages,
                VoiceDemo = dto.VoiceDemo,
                Remark = dto.Remark,
                Sort = dto.Sort,
                Creator = dto.Creator,
                CreateDate = dto.CreateDate ?? DateTime.UtcNow,
                Updater = dto.Updater,
                UpdateDate = dto.UpdateDate
            };

            _context.AiTtsVoices.Add(entity);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task UpdateAsync(TimbreDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Id)) throw new ArgumentException("音色ID不能为空", nameof(dto.Id));

            var entity = await _context.AiTtsVoices.FindAsync(dto.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException("音色不存在");
            }

            entity.TtsModelId = dto.TtsModelId;
            entity.Name = dto.Name;
            entity.TtsVoice = dto.TtsVoice;
            entity.Languages = dto.Languages;
            entity.VoiceDemo = dto.VoiceDemo;
            entity.Remark = dto.Remark;
            entity.Sort = dto.Sort;
            entity.Updater = dto.Updater;
            entity.UpdateDate = dto.UpdateDate ?? DateTime.UtcNow;

            _context.AiTtsVoices.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0) return;
            var toDelete = await _context.AiTtsVoices.Where(v => ids.Contains(v.Id)).ToListAsync();
            if (toDelete.Count == 0) return;
            _context.AiTtsVoices.RemoveRange(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TimbreDto>> GetVoiceNamesAsync(string ttsModelId, string voiceName)
        {
            var q = _context.AiTtsVoices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(ttsModelId))
            {
                q = q.Where(v => v.TtsModelId == ttsModelId);
            }

            if (!string.IsNullOrWhiteSpace(voiceName))
            {
                q = q.Where(v => v.Name.Contains(voiceName));
            }

            return await q
                .OrderBy(v => v.Sort)
                .ThenBy(v => v.Name)
                .Select(v => MapToDto(v))
                .ToListAsync();
        }

        public async Task<string> GetTimbreNameByIdAsync(string id)
        {
            var entity = await _context.AiTtsVoices.FindAsync(id);
            return entity?.Name;
        }

        private static TimbreDto MapToDto(AiTtsVoice v)
        {
            return new TimbreDto
            {
                Id = v.Id,
                TtsModelId = v.TtsModelId,
                Name = v.Name,
                TtsVoice = v.TtsVoice,
                Languages = v.Languages,
                VoiceDemo = v.VoiceDemo,
                Remark = v.Remark,
                Sort = v.Sort,
                Creator = v.Creator,
                CreateDate = v.CreateDate,
                Updater = v.Updater,
                UpdateDate = v.UpdateDate
            };
        }
    }
}