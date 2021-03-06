// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using BuildXL.Cache.ContentStore.Hashing;
using BuildXL.Cache.ContentStore.Interfaces.FileSystem;
using BuildXL.Cache.ContentStore.Interfaces.Sessions;
using BuildXL.Utilities;
using BuildXL.Utilities.Collections;

using RealPath = BuildXL.Utilities.AbsolutePath;

namespace BuildXL.Cache.ContentStore.Vfs
{
    /// <summary>
    /// Tracks the virtualized file system nodes (directories and files with content info) under a virtual root.
    /// </summary>
    internal class VfsTree
    {
        private readonly ConcurrentBigMap<string, VfsNode> _nodeMap = new ConcurrentBigMap<string, VfsNode>(keyComparer: StringComparer.OrdinalIgnoreCase);
        private readonly VfsDirectoryNode _root;
        private readonly VfsCasConfiguration _configuration;
        public PathTable PathTable { get; }

        public VfsTree(VfsCasConfiguration configuration)
        {
            PathTable = new PathTable();
            _configuration = configuration;
            _root = new VfsDirectoryNode(string.Empty, DateTime.UtcNow, null);
            _nodeMap[string.Empty] = _root;
        }

        /// <summary>
        /// Attempts to get the VFS node (directory or file) at the given VFS root relative path
        /// </summary>
        public bool TryGetNode(string relativePath, out VfsNode node)
        {
            return _nodeMap.TryGetValue(relativePath, out node);
        }

        /// <summary>
        /// Adds a file node (and any needed parent directory nodes) at the VFS root relative path
        /// </summary>
        public VfsFileNode AddFileNode(string relativePath, VfsFilePlacementData data, string realPath)
        {
            var timestamp = DateTime.UtcNow;

            if (_nodeMap.TryGetValue(relativePath, out var node))
            {
                return (VfsFileNode)node;
            }
            else
            {
                var parent = GetOrAddDirectoryNode(Path.GetDirectoryName(relativePath), allowAdd: true);
                var result = _nodeMap.GetOrAdd(relativePath, (parent, timestamp, data), (l_relativePath, l_data) =>
                {
                    return new VfsFileNode(Path.GetFileName(l_relativePath), l_data.timestamp, l_data.parent, l_data.data, RealPath.Create(PathTable, realPath));
                });

                node = result.Item.Value;

                return (VfsFileNode)node;
            }
        }

        public VfsDirectoryNode GetOrAddDirectoryNode(string relativePath, bool allowAdd = true)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return _root;
            }

            if (_nodeMap.TryGetValue(relativePath, out var node))
            {
                return (VfsDirectoryNode)node;
            }
            else if (allowAdd)
            {
                var parent = GetOrAddDirectoryNode(Path.GetDirectoryName(relativePath), allowAdd: true);
                node = _nodeMap.GetOrAdd(relativePath, parent, (l_relativePath, l_parent) =>
                {
                    return new VfsDirectoryNode(Path.GetFileName(relativePath), DateTime.UtcNow, parent);
                }).Item.Value;

                return (VfsDirectoryNode)node;
            }

            return null;
        }
    }

    public abstract class VfsNode
    {
        public readonly string Name;
        public readonly DateTime Timestamp;
        public VfsNode NextSibling;
        public VfsNode PreviousSibling;
        public readonly VfsDirectoryNode Parent;

        public virtual long Size { get; set; } = -1;
        public abstract bool IsDirectory { get; }

        public FileAttributes Attributes => IsDirectory ? FileAttributes.Directory : FileAttributes.Normal | FileAttributes.ReparsePoint;

        public VfsNode(string name, DateTime timestamp, VfsDirectoryNode parent)
        {
            Name = name;
            Timestamp = timestamp;
            Parent = parent;

            if (parent != null)
            {
                lock (parent)
                {
                    Parent.IncrementVersion();

                    NextSibling = parent.FirstChild;
                    if (parent.FirstChild != null)
                    {
                        parent.FirstChild.PreviousSibling = this;
                    }

                    Parent.FirstChild = this;
                }
            }
        }

        public void Remove()
        {
            if (Parent != null)
            {
                lock (Parent)
                {
                    Parent.IncrementVersion();

                    if (Parent.FirstChild == this)
                    {
                        Parent.FirstChild = NextSibling;
                        NextSibling.PreviousSibling = null;
                    }
                    else
                    {
                        PreviousSibling.NextSibling = NextSibling;
                        NextSibling.PreviousSibling = PreviousSibling;
                    }
                }
            }
        }
    }

    public class VfsDirectoryNode : VfsNode
    {
        public VfsNode FirstChild;
        public override bool IsDirectory => true;
        public int Version { get; private set; }

        public VfsDirectoryNode(string name, DateTime timestamp, VfsDirectoryNode parent)
            : base(name, timestamp, parent)
        {
        }

        public IEnumerable<VfsNode> EnumerateChildren()
        {
            var child = FirstChild;
            while (child != null)
            {
                yield return child;
                child = child.NextSibling;
            }
        }

        public void IncrementVersion()
        {
            Version++;
        }
    }

    public class VfsFileNode : VfsNode
    {
        public readonly RealPath RealPath;
        public readonly VfsFilePlacementData Data;
        public ContentHash Hash => Data.Hash;
        public FileRealizationMode RealizationMode => Data.RealizationMode;
        public FileAccessMode AccessMode => Data.AccessMode;
        public override long Size { get; set; }
        public override bool IsDirectory => false;

        public VfsFileNode(string name, DateTime timestamp, VfsDirectoryNode parent, VfsFilePlacementData data, RealPath realPath)
            : base(name, timestamp, parent)
        {
            Data = data;
            RealPath = realPath;
        }
    }
}
