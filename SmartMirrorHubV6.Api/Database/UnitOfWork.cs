﻿using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IComponentRepository components, IComponentSettingRepository componentSettings, IMirrorRepository mirrors, IMirrorComponentRepository mirrorComponents, IResponseHistoryRepository responseHistory, ITokenRepository tokens, IUserRepository users)
    {
        Components = components;
        ComponentSettings = componentSettings;
        Mirrors = mirrors;
        MirrorComponents = mirrorComponents;
        ResponseHistory = responseHistory;
        Tokens = tokens;
        Users = users;
    }

    public IComponentRepository Components { get; }
    public IComponentSettingRepository ComponentSettings { get; }
    public IMirrorRepository Mirrors { get; }
    public IMirrorComponentRepository MirrorComponents { get; }
    public IResponseHistoryRepository ResponseHistory { get; }
    public ITokenRepository Tokens { get; }
    public IUserRepository Users { get; }
}
