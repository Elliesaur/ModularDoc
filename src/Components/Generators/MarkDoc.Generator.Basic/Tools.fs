﻿namespace MarkDoc.Generator.Basic

open MarkDoc.Diagrams
open MarkDoc.Linkers
open MarkDoc.Members
open MarkDoc.Elements
open MarkDoc.Documentation

/// <summary>
/// Tools for composing types
/// </summary>
type Tools = {
  /// <summary>
  /// Type linker
  /// </summary>
  linker: ILinker
  /// <summary>
  /// Documentation elements creator
  /// </summary>
  creator: IElementCreator;
  /// <summary>
  /// Documentation resolver
  /// </summary>
  docResolver: IDocResolver;
  /// <summary>
  /// Type resolver
  /// </summary>
  typeResolver: IResolver
  /// <summary>
  /// Diagram resolver
  /// </summary>
  diagramResolver: IDiagramResolver
}
