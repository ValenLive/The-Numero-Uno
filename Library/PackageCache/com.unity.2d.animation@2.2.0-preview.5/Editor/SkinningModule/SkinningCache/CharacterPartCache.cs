using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.U2D.Animation
{
    [Serializable]
    internal class CharacterGroupCache : SkinningObject
    {
        [SerializeField]
        public int parentGroup;
        [SerializeField]
        public bool isVisible = true;
    }

    internal class CharacterPartCache : TransformCache
    {
        [SerializeField]
        private SpriteCache m_Sprite;
        [SerializeField]
        private List<BoneCache> m_Bones = new List<BoneCache>();
        [SerializeField]
        private bool m_IsVisible = true;
        [SerializeField]
        private int m_ParentGroup = -1;

        public int parentGroup
        {
            get { return m_ParentGroup; }
            set { m_ParentGroup = value; }
        }

        public virtual bool isVisible
        {
            get { return m_IsVisible; }
            set { m_IsVisible = value; }
        }

        public int BoneCount { get { return m_Bones.Count; } }

        public virtual SpriteCache sprite
        {
            get { return m_Sprite; }
            set { m_Sprite = value; }
        }

        public BoneCache[] bones
        {
            get { return m_Bones.ToArray(); }
            set { m_Bones = new List<BoneCache>(value); }
        }

        public BoneCache GetBone(int index)
        {
            return m_Bones[index];
        }

        public int IndexOf(BoneCache bone)
        {
            return m_Bones.IndexOf(bone);
        }

        public bool Contains(BoneCache bone)
        {
            return m_Bones.Contains(bone);
        }
    }
}