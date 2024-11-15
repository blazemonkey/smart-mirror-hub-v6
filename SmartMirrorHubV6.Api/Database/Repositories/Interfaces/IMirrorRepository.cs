﻿using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IMirrorRepository
{
    Task<Mirror[]> GetAll(bool includeComponents = false);
    Task<Mirror> GetById(int id, bool includeComponents = false);
    Task<Mirror> GetByVoiceDeviceId(string voiceDeviceId, bool includeComponents = false);
}
