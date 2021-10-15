﻿using System;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for settings of libraries
  /// </summary>
  public interface ILibrarySettings
  {
    /// <summary>
    /// Settings id
    /// </summary>
    Guid Id { get; }
  }
}