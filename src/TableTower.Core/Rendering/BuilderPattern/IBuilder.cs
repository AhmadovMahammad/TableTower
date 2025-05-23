﻿using TableTower.Core.Models;

namespace TableTower.Core.Rendering.BuilderPattern;
public interface IBuilder
{
    void SetTable(Table table);
    void BuildTitle();
    void BuildHeader();
    void BuildBody();
    void BuildFooter();
    string GetResult();
}
