﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public interface IMaterialController : IController
    {
        void setAutoUpdatePreview(bool status);
        bool getAutoUpdatePreview();
        void setChangesMade(bool status);
        bool getChangesMade();
        void preview(Material material);
        void previewClosed();
        void viewClosed();
        ReadOnlyCollection<Material> getMaterials(MaterialType type);
    }
}