using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor.U2D.Layout;
using UnityEditor.U2D.Sprites;
using UnityEngine.U2D.Common;
using Debug = UnityEngine.Debug;
using UnityEngine.UIElements;

namespace UnityEditor.U2D.Animation
{
    internal class SkinningObject : CacheObject
    {
        public SkinningCache skinningCache => owner as SkinningCache;
    }

    internal class SkinningCache : Cache
    {
        [Serializable]
        class SpriteMap : SerializableDictionary<string, SpriteCache> {}
        [Serializable]
        class MeshMap : SerializableDictionary<SpriteCache, MeshCache> {}
        [Serializable]
        class SkeletonMap : SerializableDictionary<SpriteCache, SkeletonCache> {}
        [Serializable]
        class ToolMap : SerializableDictionary<Tools, BaseTool> {}
        [Serializable]
        class MeshPreviewMap : SerializableDictionary<SpriteCache, MeshPreviewCache> {}
        [Serializable]
        class CharacterPartMap : SerializableDictionary<SpriteCache, CharacterPartCache> {}

        [SerializeField]
        SkinningEvents m_Events = new SkinningEvents();
        [SerializeField]
        List<BaseTool> m_Tools = new List<BaseTool>();
        [SerializeField]
        SpriteMap m_SpriteMap = new SpriteMap();
        [SerializeField]
        MeshMap m_MeshMap = new MeshMap();
        [SerializeField]
        MeshPreviewMap m_MeshPreviewMap = new MeshPreviewMap();
        [SerializeField]
        SkeletonMap m_SkeletonMap = new SkeletonMap();
        [SerializeField]
        CharacterPartMap m_CharacterPartMap = new CharacterPartMap();
        [SerializeField]
        ToolMap m_ToolMap = new ToolMap();
        [SerializeField]
        SelectionTool m_SelectionTool;
        [SerializeField]
        CharacterCache m_Character;
        [SerializeField]
        bool m_BonesReadOnly;
        [SerializeField]
        SkinningMode m_Mode = SkinningMode.SpriteSheet;
        [SerializeField]
        BaseTool m_SelectedTool;
        [SerializeField]
        SpriteCache m_SelectedSprite;
        [SerializeField]
        SkeletonSelection m_SkeletonSelection = new SkeletonSelection();
        [SerializeField] 
        ISkinningCachePersistentState m_State;

        StringBuilder m_StringBuilder = new StringBuilder();
        
        public BaseTool selectedTool
        {
            get => m_SelectedTool;
            set
            {
                m_SelectedTool = value;
                try
                {
                    m_State.lastUsedTool = m_ToolMap[value];
                }
                catch (KeyNotFoundException)
                {
                    m_State.lastUsedTool = Tools.EditPose;
                }
            }
        }

        public virtual SkinningMode mode
        {
            get => m_Mode;
            set
            {
                m_Mode = CheckModeConsistency(value);
                m_State.lastMode = m_Mode;
            }
        }

        public SpriteCache selectedSprite
        {
            get => m_SelectedSprite;
            set
            {
                m_SelectedSprite = value;
                m_State.lastSpriteId = m_SelectedSprite ? m_SelectedSprite.id : String.Empty;
            }
        }

        public float brushSize
        {
            get => m_State.lastBrushSize;
            set => m_State.lastBrushSize = value;
        }

        public float brushHardness
        {
            get => m_State.lastBrushHardness;
            set => m_State.lastBrushHardness = value;
        }

        public float brushStep
        {
            get => m_State.lastBrushStep;
            set => m_State.lastBrushStep = value;
        }

        public int visibilityToolIndex
        {
            get => m_State.lastVisibilityToolIndex;
            set => m_State.lastVisibilityToolIndex = value;
        }
        
        public SkeletonSelection skeletonSelection => m_SkeletonSelection;

        public IndexedSelection vertexSelection => m_State.lastVertexSelection;

        public SkinningEvents events => m_Events;

        public SelectionTool selectionTool => m_SelectionTool;

        public SpriteCache[] GetSprites()
        {
            return m_SpriteMap.Values.ToArray();
        }

        public virtual CharacterCache character => m_Character;

        public bool hasCharacter => character != null;

        public bool bonesReadOnly => m_BonesReadOnly;

        public bool applyingChanges
        {
            get;
            set;
        }

        SkinningMode CheckModeConsistency(SkinningMode skinningMode)
        {
            if (skinningMode == SkinningMode.Character && hasCharacter == false)
                skinningMode = SkinningMode.SpriteSheet;

            return skinningMode;
        }

        public void Create(ISpriteEditorDataProvider spriteEditor, ISkinningCachePersistentState state)
        {
            Clear();

            var dataProvider = spriteEditor.GetDataProvider<ISpriteEditorDataProvider>();
            var boneProvider = spriteEditor.GetDataProvider<ISpriteBoneDataProvider>();
            var meshProvider = spriteEditor.GetDataProvider<ISpriteMeshDataProvider>();
            var spriteRects = dataProvider.GetSpriteRects();
            var textureProvider = spriteEditor.GetDataProvider<ITextureDataProvider>();

            m_State = state;
            m_State.lastTexture = textureProvider.texture;

            for (var i = 0; i < spriteRects.Length; i++)
            {
                var spriteRect = spriteRects[i];
                var sprite = CreateSpriteCache(spriteRect);
                CreateSkeletonCache(sprite, boneProvider);
                CreateMeshCache(sprite, meshProvider, textureProvider);
                CreateMeshPreviewCache(sprite);
            }

            CreateCharacter(spriteEditor);
        }

        public void CreateToolCache(ISpriteEditor spriteEditor, LayoutOverlay layoutOverlay)
        {
            var spriteEditorDataProvider = spriteEditor.GetDataProvider<ISpriteEditorDataProvider>();
            var skeletonTool = CreateCache<SkeletonTool>();
            var meshTool = CreateCache<MeshTool>();

            skeletonTool.Initialize(layoutOverlay);
            meshTool.Initialize(layoutOverlay);

            m_ToolMap.Add(Tools.EditPose, CreateSkeletonTool<SkeletonToolWrapper>(skeletonTool, SkeletonMode.EditPose, false, layoutOverlay));
            m_ToolMap.Add(Tools.EditJoints, CreateSkeletonTool<SkeletonToolWrapper>(skeletonTool, SkeletonMode.EditJoints, true, layoutOverlay));
            m_ToolMap.Add(Tools.CreateBone, CreateSkeletonTool<SkeletonToolWrapper>(skeletonTool, SkeletonMode.CreateBone, true, layoutOverlay));
            m_ToolMap.Add(Tools.SplitBone, CreateSkeletonTool<SkeletonToolWrapper>(skeletonTool, SkeletonMode.SplitBone, true, layoutOverlay));
            m_ToolMap.Add(Tools.ReparentBone, CreateSkeletonTool<BoneReparentTool>(skeletonTool, SkeletonMode.EditPose, false, layoutOverlay));
            m_ToolMap.Add(Tools.CharacterPivotTool, CreateSkeletonTool<PivotTool>(skeletonTool, SkeletonMode.Disabled, false, layoutOverlay));

            m_ToolMap.Add(Tools.EditGeometry, CreateMeshTool<MeshToolWrapper>(skeletonTool, meshTool, SpriteMeshViewMode.EditGeometry, SkeletonMode.Disabled, layoutOverlay));
            m_ToolMap.Add(Tools.CreateVertex, CreateMeshTool<MeshToolWrapper>(skeletonTool, meshTool, SpriteMeshViewMode.CreateVertex, SkeletonMode.Disabled, layoutOverlay));
            m_ToolMap.Add(Tools.CreateEdge, CreateMeshTool<MeshToolWrapper>(skeletonTool, meshTool, SpriteMeshViewMode.CreateEdge, SkeletonMode.Disabled, layoutOverlay));
            m_ToolMap.Add(Tools.SplitEdge, CreateMeshTool<MeshToolWrapper>(skeletonTool, meshTool, SpriteMeshViewMode.SplitEdge, SkeletonMode.Disabled, layoutOverlay));
            m_ToolMap.Add(Tools.GenerateGeometry, CreateMeshTool<GenerateGeometryTool>(skeletonTool, meshTool, SpriteMeshViewMode.EditGeometry, SkeletonMode.EditPose, layoutOverlay));
            var copyTool = CreateTool<CopyTool>();
            copyTool.Initialize(layoutOverlay);
            copyTool.pixelsPerUnit = spriteEditorDataProvider.pixelsPerUnit;
            copyTool.skeletonTool = skeletonTool;
            copyTool.meshTool = meshTool;
            m_ToolMap.Add(Tools.CopyPaste, copyTool);

            CreateWeightTools(skeletonTool, meshTool, layoutOverlay);

            m_SelectionTool = CreateTool<SelectionTool>();
            m_SelectionTool.spriteEditor = spriteEditor;
            m_SelectionTool.Initialize(layoutOverlay);
            m_SelectionTool.Activate();

            var visibilityTool = CreateTool<VisibilityTool>();
            visibilityTool.Initialize(layoutOverlay);
            visibilityTool.skeletonTool = skeletonTool;
            m_ToolMap.Add(Tools.Visibility, visibilityTool);

            var switchModeTool = CreateTool<SwitchModeTool>();
            m_ToolMap.Add(Tools.SwitchMode, switchModeTool);
        }

        public void RestoreFromPersistentState()
        {
            mode = m_State.lastMode;
            events.skinningModeChanged.Invoke(mode);

            var hasLastSprite = m_SpriteMap.TryGetValue(m_State.lastSpriteId, out var lastSprite);
            if (hasLastSprite)
            {
                selectedSprite = lastSprite;
            }
            else
            {
                vertexSelection.Clear();
            }

            if (m_ToolMap.TryGetValue(m_State.lastUsedTool, out var baseTool))
            {
                selectedTool = baseTool;
            }
            else if (m_ToolMap.TryGetValue(Tools.EditPose, out baseTool))
            {
                selectedTool = baseTool;
            }

            var visibilityTool = m_ToolMap[Tools.Visibility];
            if (m_State.lastVisibilityToolActive)
            {
                visibilityTool.Activate();
            }
        }

        public void RestoreToolStateFromPersistentState()
        {
            events.boneSelectionChanged.RemoveListener(BoneSelectionChanged);
            events.skeletonPreviewPoseChanged.RemoveListener(SkeletonPreviewPoseChanged);
            events.toolChanged.RemoveListener(ToolChanged);

            SkeletonCache skeleton = null;
            if (hasCharacter)
                skeleton = character.skeleton;
            else if (selectedSprite != null)
                skeleton = selectedSprite.GetSkeleton();

            skeletonSelection.Clear();
            if (skeleton != null && m_State.lastBoneSelectionIds.Count > 0)
            {
                bool selectionChanged = false;
                foreach (var bone in skeleton.bones)
                {
                    var id = GetBoneNameHash(m_StringBuilder, bone);
                    if (m_State.lastBoneSelectionIds.Contains(id))
                    {
                        skeletonSelection.Select(bone, true);
                        selectionChanged = true;
                    }
                }
                if (selectionChanged)
                    events.boneSelectionChanged.Invoke();
            }

            if (m_State.lastPreviewPose.Count > 0)
            {
                if (hasCharacter)
                {
                    UpdatePoseFromPersistentState(character.skeleton, null);
                }
                foreach (var sprite in m_SkeletonMap.Keys)
                {
                    UpdatePoseFromPersistentState(m_SkeletonMap[sprite], sprite);
                }
            }

            if (m_State.lastBoneVisibility.Count > 0)
            {
                if (hasCharacter)
                {
                    UpdateVisibilityFromPersistentState(character.skeleton, null);
                }

                foreach (var sprite in m_SkeletonMap.Keys)
                {
                    UpdateVisibilityFromPersistentState(m_SkeletonMap[sprite], sprite);
                }
            }

            if (m_State.lastSpriteVisibility.Count > 0 && hasCharacter)
            {
                foreach (var characterPart in character.parts)
                {
                    if (m_State.lastSpriteVisibility.TryGetValue(characterPart.sprite.id, out var visibility))
                    {
                        characterPart.isVisible = visibility;
                    }
                }

                foreach (var characterGroup in character.groups)
                {
                    var groupHash = GetCharacterGroupHash(m_StringBuilder, characterGroup, character);
                    if (m_State.lastGroupVisibility.TryGetValue(groupHash, out var visibility))
                    {
                        characterGroup.isVisible = visibility;
                    }
                }
            }

            events.boneSelectionChanged.AddListener(BoneSelectionChanged);
            events.skeletonPreviewPoseChanged.AddListener(SkeletonPreviewPoseChanged);
            events.toolChanged.AddListener(ToolChanged);
        }

        void UpdatePoseFromPersistentState(SkeletonCache skeleton, SpriteCache sprite)
        {
            var poseChanged = false;
            foreach (var bone in skeleton.bones)
            {
                var id = GetBoneNameHash(m_StringBuilder, bone, sprite);
                if (m_State.lastPreviewPose.TryGetValue(id, out var pose))
                {
                    bone.localPose = pose;
                    poseChanged = true;
                }
            }
            if (poseChanged)
            {
                skeleton.SetPosePreview();
                events.skeletonPreviewPoseChanged.Invoke(skeleton);    
            }
        }

        void UpdateVisibilityFromPersistentState(SkeletonCache skeleton, SpriteCache sprite)
        {
            foreach (var bone in skeleton.bones)
            {
                var id = GetBoneNameHash(m_StringBuilder, bone, sprite);
                if (m_State.lastBoneVisibility.TryGetValue(id, out var visibility))
                {
                    bone.isVisible = visibility;
                }
            }
        }

        const string k_NameSeparator = "/";

        int GetBoneNameHash(StringBuilder sb, BoneCache bone, SpriteCache sprite = null)
        {
            sb.Clear();
            BuildBoneName(sb, bone);
            sb.Append(k_NameSeparator);
            if (sprite != null)
            {
                sb.Append(sprite.id);
            }
            else
            {
                sb.Append(0);
            }
            return Animator.StringToHash(sb.ToString());
        }

        static void BuildBoneName(StringBuilder sb, BoneCache bone)
        {
            if (bone.parentBone != null)
            {
                BuildBoneName(sb, bone.parentBone);
                sb.Append(k_NameSeparator);
            }
            sb.Append(bone.name);
        }

        static int GetCharacterGroupHash(StringBuilder sb, CharacterGroupCache characterGroup, CharacterCache characterCache)
        {
            sb.Clear();
            BuildGroupName(sb, characterGroup, characterCache);
            return Animator.StringToHash(sb.ToString());
        }

        static void BuildGroupName(StringBuilder sb, CharacterGroupCache group, CharacterCache characterCache)
        {
            if (group.parentGroup >= 0 && group.parentGroup < characterCache.groups.Length)
            {
                BuildGroupName(sb, characterCache.groups[group.parentGroup], characterCache);
                sb.Append(k_NameSeparator);
            }
            sb.Append(group.order);
        }

        void BoneSelectionChanged()
        {
            m_State.lastBoneSelectionIds.Clear();
            m_State.lastBoneSelectionIds.Capacity = skeletonSelection.elements.Length;
            for (var i = 0; i < skeletonSelection.elements.Length; ++i)
            {
                var bone = skeletonSelection.elements[i];
                m_State.lastBoneSelectionIds.Add(GetBoneNameHash(m_StringBuilder, bone));
            }
        }

        void SkeletonPreviewPoseChanged(SkeletonCache sc)
        {
            if (applyingChanges)
                return;

            m_State.lastPreviewPose.Clear();
            if (hasCharacter)
            {
                StorePersistentStatePoseForSkeleton(character.skeleton, null);
            }
            foreach (var sprite in m_SkeletonMap.Keys)
            {
                StorePersistentStatePoseForSkeleton(m_SkeletonMap[sprite], sprite);
            }
        }

        void StorePersistentStatePoseForSkeleton(SkeletonCache skeleton, SpriteCache sprite)
        {
            foreach (var bone in skeleton.bones)
            {
                var id = GetBoneNameHash(m_StringBuilder, bone, sprite);
                if (bone.NotInDefaultPose())
                {
                    m_State.lastPreviewPose[id] = bone.localPose;
                }
            }
        }

        internal void Revert()
        {
            m_State.lastVertexSelection.Clear();
        }

        internal void BoneVisibilityChanged()
        {
            if (applyingChanges)
                return;

            m_State.lastBoneVisibility.Clear();
            if (hasCharacter)
            {
                StorePersistentStateVisibilityForSkeleton(character.skeleton, null);
            }
            foreach (var sprite in m_SkeletonMap.Keys)
            {
                StorePersistentStateVisibilityForSkeleton(m_SkeletonMap[sprite], sprite);
            }
        }

        void StorePersistentStateVisibilityForSkeleton(SkeletonCache skeleton, SpriteCache sprite)
        {
            foreach (var bone in skeleton.bones)
            {
                var id = GetBoneNameHash(m_StringBuilder, bone, sprite);
                m_State.lastBoneVisibility[id] = bone.isVisible;
            }
        }

        internal void BoneExpansionChanged(BoneCache[] boneCaches)
        {
            if (applyingChanges)
                return;

            m_State.lastBoneExpansion.Clear();
            if (hasCharacter)
            {
                foreach (var bone in boneCaches)
                {
                    if (character.skeleton.bones.Contains(bone))
                    {
                        var id = GetBoneNameHash(m_StringBuilder, bone, null);
                        m_State.lastBoneExpansion[id] = true;    
                    }
                }
            }

            foreach (var sprite in m_SkeletonMap.Keys)
            {
                var skeleton = m_SkeletonMap[sprite];
                foreach (var bone in boneCaches)
                {
                    if (skeleton.bones.Contains(bone))
                    {
                        var id = GetBoneNameHash(m_StringBuilder, bone, sprite);
                        m_State.lastBoneExpansion[id] = true;    
                    }
                }
            }
        }

        internal BoneCache[] GetExpandedBones()
        {
            var expandedBones = new HashSet<BoneCache>();
            if (m_State.lastBoneExpansion.Count > 0)
            {
                if (hasCharacter)
                {
                    foreach (var bone in character.skeleton.bones)
                    {
                        var id = GetBoneNameHash(m_StringBuilder, bone, null);
                        if (m_State.lastBoneExpansion.TryGetValue(id, out var expanded))
                        {
                            expandedBones.Add(bone);
                        }    
                    }
                }
                foreach (var sprite in m_SkeletonMap.Keys)
                {
                    var skeleton = m_SkeletonMap[sprite];
                    foreach (var bone in skeleton.bones)
                    {
                        var id = GetBoneNameHash(m_StringBuilder, bone, sprite);
                        if (m_State.lastBoneExpansion.TryGetValue(id, out var expanded))
                        {
                            expandedBones.Add(bone);
                        }    
                    }
                }
            }
            return expandedBones.ToArray();
        }

        internal void SpriteVisibilityChanged(CharacterPartCache cc)
        {
            m_State.lastSpriteVisibility[cc.sprite.id] = cc.isVisible;
        }

        internal void GroupVisibilityChanged(CharacterGroupCache gc)
        {
            if (!hasCharacter)
                return;

            var groupHash = GetCharacterGroupHash(m_StringBuilder, gc, character);
            m_State.lastGroupVisibility[groupHash] = gc.isVisible;
        }

        void Clear()
        {
            Destroy();
            m_Tools.Clear();
            m_SpriteMap.Clear();
            m_MeshMap.Clear();
            m_MeshPreviewMap.Clear();
            m_SkeletonMap.Clear();
            m_ToolMap.Clear();
            m_CharacterPartMap.Clear();
        }

        public SpriteCache GetSprite(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            m_SpriteMap.TryGetValue(id, out var sprite);
            return sprite;
        }

        public virtual MeshCache GetMesh(SpriteCache sprite)
        {
            if (sprite == null)
                return null;

            m_MeshMap.TryGetValue(sprite, out var mesh);
            return mesh;
        }

        public virtual MeshPreviewCache GetMeshPreview(SpriteCache sprite)
        {
            if (sprite == null)
                return null;

            m_MeshPreviewMap.TryGetValue(sprite, out var meshPreview);
            return meshPreview;
        }

        public SkeletonCache GetSkeleton(SpriteCache sprite)
        {
            if (sprite == null)
                return null;

            m_SkeletonMap.TryGetValue(sprite, out var skeleton);
            return skeleton;
        }

        public virtual CharacterPartCache GetCharacterPart(SpriteCache sprite)
        {
            if (sprite == null)
                return null;

            m_CharacterPartMap.TryGetValue(sprite, out var part);
            return part;
        }

        public SkeletonCache GetEffectiveSkeleton(SpriteCache sprite)
        {
            if (mode == SkinningMode.SpriteSheet)
                return GetSkeleton(sprite);

            if (hasCharacter)
                return character.skeleton;

            return null;
        }

        public BaseTool GetTool(Tools tool)
        {
            m_ToolMap.TryGetValue(tool, out var t);
            return t;
        }

        public override void BeginUndoOperation(string operationName)
        {
            if (isUndoOperationSet == false)
            {
                base.BeginUndoOperation(operationName);
                undo.RegisterCompleteObjectUndo(m_State, operationName);
            }
        }

        public UndoScope UndoScope(string operationName, bool incrementGroup = false)
        {
            return new UndoScope(this, operationName, incrementGroup);
        }

        public DisableUndoScope DisableUndoScope()
        {
            return new DisableUndoScope(this);
        }

        public T CreateTool<T>() where T : BaseTool
        {
            var tool = CreateCache<T>();
            m_Tools.Add(tool);
            return tool;
        }

        void UpdateCharacterPart(CharacterPartCache characterPart)
        {
            var sprite = characterPart.sprite;
            var characterPartBones = characterPart.bones;
            var newBones = new List<BoneCache>(characterPartBones);
            newBones.RemoveAll(b => b == null || IsRemoved(b) || b.skeleton != character.skeleton);
            var removedBonesCount = characterPartBones.Length - newBones.Count;

            characterPartBones = newBones.ToArray();
            characterPart.bones = characterPartBones;
            sprite.UpdateMesh(characterPartBones);

            if (removedBonesCount > 0)
                sprite.SmoothFill();
        }

        public void CreateSpriteSheetSkeletons()
        {
            Debug.Assert(character != null);

            using (new DefaultPoseScope(character.skeleton))
            {
                var characterParts = character.parts;

                foreach (var characterPart in characterParts)
                    CreateSpriteSheetSkeleton(characterPart);
            }

            SyncSpriteSheetSkeletons();
        }

        public void SyncSpriteSheetSkeletons()
        {
            Debug.Assert(character != null);

            var char¨      8¦    #      [   n>¾¤XyÚ              l < A n i m a t i o n s . A n i m a t o r J o b E x t e n s i o n s . A d d J o b D e p e n d e n c y . h t m l ¨      8¦    #    ¨ [   n>¾¤XyÚ €            l < A n i m a t i o n s . A n i m a t o r J o b E x t e n s i o n s . A d d J o b D e p e n d e n c y . h t m l €      9¦    #    P‘ [   n>¾¤XyÚ               B < A p p l i c a t i o n M e m o r y U s a g e C h a n g e . h t m l   €      9¦    #    Ð‘ [   n>¾¤XyÚ              B < A p p l i c a t i o n M e m o r y U s a g e C h a n g e . h t m l   €      9¦    #    P’ [   n>¾¤XyÚ €            B < A p p l i c a t i o n M e m o r y U s a g e C h a n g e . h t m l   ˆ      :¦    #    Ð’ [   n>¾¤XyÚ               J < P r o f i l i n g . F r a m e D a t a V i e w - t h r e a d I d . h t m l   ˆ      :¦    #    X“ [   n>¾¤XyÚ              J < P r o f i l i n g . F r a m e D a t a V i e w - t h r e a d I d . h t m l   ˆ      :¦    #    à“ [   n>¾¤XyÚ €            J < P r o f i l i n g . F r a m e D a t a V i e w - t h r e a d I d . h t m l          ;¦    #    h” [   n>¾¤XyÚ               b < I M G U I . C o n t r o l s . B o x B o u n d s H a n d l e . D r a w W i r e f r a m e . h t m l          ;¦    #    • [   n>¾¤XyÚ              b < I M G U I . C o n t r o l s . B o x B o u n d s H a n d l e . D r a w W i r e f r a m e . h t m l          ;¦    #    ¨• [   n>¾¤XyÚ €            b < I M G U I . C o n t r o l s . B o x B o u n d s H a n d l e . D r a w W i r e f r a m e . h t m l         <¦    #    H– [   d¿¤XyÚ               P < C a m e r a . R e n d e r R e q u e s t O u t p u t S p a c e . U V 0 . h t m l           <¦    #    Ø– [   d¿¤XyÚ              P < C a m e r a . R e n d e r R e q u e s t O u t p u t S p a c e . U V 0 . h t m l           <¦    #    h— [   d¿¤XyÚ €            P < C a m e r a . R e n d e r R e q u e s t O u t p u t S p a c e . U V 0 . h t m l           =¦    #    ø— [   rx¿¤XyÚ               N < P a c k a g e M a n a g e r . R e q u e s t s . A d d R e q u e s t . h t m l             =¦    #    ˆ˜ [   rx¿¤XyÚ              N < P a c k a g e M a n a g e r . R e q u e s t s . A d d R e q u e s t . h t m l             =¦    #    ™ [   rx¿¤XyÚ €            N < P a c k a g e M a n a g e r . R e q u e s t s . A d d R e q u e s t . h t m l       €      >¦    #    ¨™ [   rx¿¤XyÚ               > < T e x t u r e F o r m a t . A S T C _ R G B _ 6 x 6 . h t m l       €      >¦    #    (š [   rx¿¤XyÚ              > < T e x t u r e F o r m a t . A S T C _ R G B _ 6 x 6 . h t m l       €      >¦    #    ¨š [   rx¿¤XyÚ €            > < T e x t u r e F o r m a t . A S T C _ R G B _ 6 x 6 . h t m l       ¸      ?¦    #    (› [   ˆÅ¿¤XyÚ               v < N e t w o r k i n g . P l a y e r C o n n e c t i o n . P l a y e r C o n n e c t i o n G U I U t i l i t y . h t m l       ¸      ?¦    #    à› [   ˆÅ¿¤XyÚ              v < N e t w o r k i n g . P l a y e r C o n n e c t i o n . P l a y e r C o n n e c t i o n G U I U t i l i t y . h t m l       ¸      ?¦    #    ˜œ [   ©ì¿¤XyÚ €            v < N e t w o r k i n g . P l a y e r C o n n e c t i o n . P l a y e r C o n n e c t i o n G U I U t i l i t y . h t m l       x      @¦    #    P [   ©ì¿¤XyÚ               6 < S h a d e r I n f o - h a s W a r n i n g s . h t m l       x      @¦    #    È [   ©ì¿¤XyÚ              6 < S h a d e r I n f o - h a s W a r n i n g s . h t m l       x      @¦    #    @ž [   ©ì¿¤XyÚ €            6 < S h a d e r I n f o - h a s W a r n i n g s . h t m l       ˆ      A¦    #    ¸ž [   ®À¤XyÚ               J < S h a d e r V a r i a n t C o l l e c t i o n . C o n t a i n s . h t m l   ˆ      A¦    #    @Ÿ [   ®À¤XyÚ              J < S h a d e r V a r i a n t C o l l e c t i o n . C o n t a i n s . h t m l                                                           ˆ      A¦    #       [   ®À¤XyÚ €            J < S h a d e r V a r i a n t C o l l e c t i o n . C o n t a i n s . h t m l   ¨      B¦    #    ˆ  [   É:À¤XyÚ               f < T e r r a i n T o o l s . P a i n t C o n t e x t - h e i g h t W o r l d S p a c e S i z e . h t m l       ¨      B¦    #    0¡ [   É:À¤XyÚ              f < T e r r a i n T o o l s . P a i n t C o n t e x t - h e i g h t W o r l d S p a c e S i z e . h t m l       ¨      B¦    #    Ø¡ [   É:À¤XyÚ €            f < T e r r a i n T o o l s . P a i n t C o n t e x t - h e i g h t W o r l d S p a c e S i z e . h t m l       €      C¦    #    €¢ [   ÔaÀ¤XyÚ               > < V i d e o C l i p I m p o r t e r - s R G B C l i p . h t m l       €      C¦    #     £ [   ÔaÀ¤XyÚ              > < V i d e o C l i p I m p o r t e r - s R G B C l i p . h t m l       €      C¦    #    €£ [   ÔaÀ¤XyÚ €            > < V i d e o C l i p I m p o r t e r - s R G B C l i p . h t m l       Ø      D¦    #     ¤ [   îˆÀ¤XyÚ               – < P r o f i l i n g . M e m o r y . E x p e r i m e n t a l . M a n a g e d M e m o r y S e c t i o n E n t r i e s - s t a r t A d d r e s s . h t m l       Ø      D¦    #    Ø¤ [   îˆÀ¤XyÚ              – < P r o f i l i n g . M e m o r y . E x p e r i m e n t a l . M a n a g e d M e m o r y S e c t i o n E n t r i e s - s t a r t A d d r e s s . h t m l       Ø      D¦    #    °¥ [   îˆÀ¤XyÚ €            – < P r o f i l i n g . M e m o r y . E x p e r i m e n t a l . M a n a g e d M e m o r y S e c t i o n E n t r i e s - s t a r t A d d r e s s . h t m l       ˆ      E¦    #    ˆ¦ [   ÷¯À¤XyÚ               J < V i d e o . V i d e o A s p e c t R a t i o . N o S c a l i n g . h t m l   ˆ      E¦    #    § [   ÷¯À¤XyÚ              J < V i d e o . V i d e o A s p e c t R a t i o . N o S c a l i n g . h t m l   ˆ      E¦    #    ˜§ [   ÷¯À¤XyÚ €            J < V i d e o . V i d e o A s p e c t R a t i o . N o S c a l i n g . h t m l   °      F¦    #     ¨ [   ÷¯À¤XyÚ               p < i O S . X c o d e . P r o j e c t C a p a b i l i t y M a n a g e r . A d d I n A p p P u r c h a s e . h t m l     °      F¦    #    Ð¨ [   
×À¤XyÚ              p < i O S . X c o d e . P r o j e c t C a p a b i l i t y M a n a g e r . A d d I n A p p P u r c h a s e . h t m l     °      F¦    #    €© [   
×À¤XyÚ €            p < i O S . X c o d e . P r o j e c t C a p a b i l i t y M a n a g e r . A d d I n A p p P u r c h a s e . h t m l     ˜      G¦    #    0ª [   
×À¤XyÚ               V < B u i l d A s s e t B u n d l e s P a r a m e t e r s - o u t p u t P a t h . h t m l       ˜      G¦    #    Èª [   þÀ¤XyÚ              V < B u i l d A s s e t B u n d l e s P a r a m e t e r s - o u t p u t P a t h . h t m l       X      è¥    ¡£    `« [   þÀ¤XyÚ               < l i b c + + - m t . a       X      è¥    ¡£    ¸« [   þÀ¤XyÚ €             < l i b c + + - m t . a       ˜      G¦    #    ¬ [   þÀ¤XyÚ €            V < B u i l d A s s e t B u n d l e s P a r a m e t e r s - o u t p u t P a t h . h t m l       `      H¦    ¡£    ¨¬ [   þÀ¤XyÚ               " < l i b c + + - n o e x c e p t . a   `      I¦    #    ­ [   þÀ¤XyÚ                < R e n d e r M o d e . h t m l       `      I¦    #    h­ [   )%Á¤XyÚ               < R e n d e r M o d e . h t m l       `      I¦    #    È­ [   )%Á¤XyÚ €             < R e n d e r M o d e . h t m l       `      H¦    ¡£    (® [   )%Á¤XyÚ              " < l i b c + + - n o e x c e p t . a          J¦    #    ˆ® [   GLÁ¤XyÚ               ` < I A p p l y R e v e r t P r o p e r t y C o n t e x t M e n u I t e m P r o v i d e r . h t m l            J¦    #    (¯ [   GLÁ¤XyÚ              ` < I A p p l y R e v e r t P r o p e r t y C o n t e x t M e n u I t e m P r o v i d e r . h t m l                                                                    J¦    #     ° [   4qÁ¤XyÚ €            ` < I A p p l y R e v e r t P r o p e r t y C o n t e x t M e n u I t e m P r o v i d e r . h t m l     È      K¦    #     ° [   4qÁ¤XyÚ               † < I M G U I . C o n t r o l s . P r i m i t i v e B o u n d s H a n d l e . H a n d l e D i r e c t i o n . P o s i t i v e X . h t m l       È      K¦    #    h± [   4qÁ¤XyÚ              † < I M G U I . C o n t r o l s . P r i m i t i v e B o u n d s H a n d l e . H a n d l e D i r e c t i o n . P o s i t i v e X . h t m l       È      K¦    #    0² [   G˜Á¤XyÚ €            † < I M G U I . C o n t r o l s . P r i m i t i v e B o u n d s H a n d l e . H a n d l e D i r e c t i o n . P o s i t i v e X . h t m l       ˜      L¦    #    ø² [   G˜Á¤XyÚ               V < A n i m a t i o n s . A n i m a t i o n L a y e r M i x e r P l a y a b l e . h t m l       ˜      L¦    #    ³ [   U¿Á¤XyÚ              V < A n i m a t i o n s . A n i m a t i o n L a y e r M i x e r P l a y a b l e . h t m l       ˜      L¦    #    (´ [   U¿Á¤XyÚ €            V < A n i m a t i o n s . A n i m a t i o n L a y e r M i x e r P l a y a b l e . h t m l             M¦    #    À´ [   U¿Á¤XyÚ               R < A s s e t I m p o r t e r s . S p r i t e I m p o r t D a t a - r e c t . h t m l         M¦    #    Pµ [   cæÁ¤XyÚ              R < A s s e t I m p o r t e r s . S p r i t e I m p o r t D a t a - r e c t . h t m l         M¦    #    àµ [   cæÁ¤XyÚ €            R < A s s e t I m p o r t e r s . S p r i t e I m p o r t D a t a - r e c t . h t m l   °      N¦    #    p¶ [   cæÁ¤XyÚ               r < U I E l e m e n t s . V i s u a l E l e m e n t E x t e n s i o n s . R e m o v e M a n i p u l a t o r . h t m l   °      N¦    #     · [   cæÁ¤XyÚ              r < U I E l e m e n t s . V i s u a l E l e m e n t E x t e n s i o n s . R e m o v e M a n i p u l a t o r . h t m l   °      N¦    #    Ð· [   †Â¤XyÚ €            r < U I E l e m e n t s . V i s u a l E l e m e n t E x t e n s i o n s . R e m o v e M a n i p u l a t o r . h t m l   ˜      O¦    #    €¸ [   †Â¤XyÚ               V < i O S . X c o d e . P l i s t D o c u m e n t . R e a d F r o m S t r e a m . h t m l       ˜      O¦    #    ¹ [   †Â¤XyÚ              V < i O S . X c o d e . P l i s t D o c u m e n t . R e a d F r o m S t r e a m . h t m l       ˜      O¦    #    °¹ [   v5Â¤XyÚ €            V < i O S . X c o d e . P l i s t D o c u m e n t . R e a d F r o m S t r e a m . h t m l       p      P¦    #    Hº [   v5Â¤XyÚ               . < N e t w o r k . D i s c o n n e c t . h t m l       p      P¦    #    ¸º [   v5Â¤XyÚ              . < N e t w o r k . D i s c o n n e c t . h t m l       p      P¦    #    (» [   ãpÂ¤XyÚ €            . < N e t w o r k . D i s c o n n e c t . h t m l       €      Q¦    #    ˜» [   ãpÂ¤XyÚ               B < A I . N a v M e s h L i n k I n s t a n c e - v a l i d . h t m l   €      Q¦    #    ¼ [   …Â¤XyÚ              B < A I . N a v M e s h L i n k I n s t a n c e - v a l i d . h t m l   €      Q¦    #    ˜¼ [   …Â¤XyÚ €            B < A I . N a v M e s h L i n k I n s t a n c e - v a l i d . h t m l   x      R¦    #    ½ [   …Â¤XyÚ               8 < T i l e m a p s . T i l e D a t a - f l a g s . h t m l     x      R¦    #    ½ [   …Â¤XyÚ              8 < T i l e m a p s . T i l e D a t a - f l a g s . h t m l     x      R¦    #    ¾ [   …Â¤XyÚ €            8 < T i l e m a p s . T i l e D a t a - f l a g s . h t m l     ¨      S¦    #    €¾ [   …Â¤XyÚ               l < U I E l e m e n t s . U n s i g n e d I n t e g e r F i e l d - i n p u t U s s C l a s s N a m e . h t m l ¨      S¦    #    (¿ [   êÓÂ¤XyÚ              l < U I E l e m e n t s . U n s i g n e d I n t e g e r F i e l d - i n p u t U s s C l a s s N a m e . h t m l                                                 ¨      S¦    #     À [   êÓÂ¤XyÚ €            l < U I E l e m e n t s . U n s i g n e d I n t e g e r F i e l d - i n p u t U s s C l a s s N a m e . h t m l €      T¦    #    ¨À [   êÓÂ¤XyÚ               @ < H a n d l e s . D r a w A A C o n v e x P o l y g o n . h t m l     €      T¦    #    (Á [   êÓÂ¤XyÚ              @ < H a n d l e s . D r a w A A C o n v e x P o l y g o n . h t m l     €      T¦    #    ¨Á [   êÓÂ¤XyÚ €            @ < H a n d l e s . D r a w A A C o n v e x P o l y g o n . h t m l     ¨      U¦    #    (Â [   êÓÂ¤XyÚ               f < T e r r a i n L a y e r U t i l i t y . V a l i d a t e N o r m a l M a p T e x t u r e U I . h t m l       ¨      U¦    #    ÐÂ [   êÓÂ¤XyÚ              f < T e r r a i n L a y e r U t i l i t y . V a l i d a t e N o r m a l M a p T e x t u r e U I . h t m l       ¨      U¦    #    xÃ [   êÓÂ¤XyÚ €            f < T e r r a i n L a y e r U t i l i t y . V a l i d a t e N o r m a l M a p T e x t u r e U I . h t m l       ˆ      V¦    #     Ä [   êÓÂ¤XyÚ               F < U I E l e m e n t s . T o o l b a r M e n u - v a r i a n t . h t m l       ˆ      V¦    #    ¨Ä [   êÓÂ¤XyÚ              F < U I E l e m e n t s . T o o l b a r M e n u - v a r i a n t . h t m l       ˆ      V¦    #    0Å [   êÓÂ¤XyÚ €            F < U I E l e m e n t s . T o o l b a r M e n u - v a r i a n t . h t m l       ˆ      W¦    #    ¸Å [   êÓÂ¤XyÚ               J < A n a l y t i c s . A n a l y t i c s E v e n t P r i o r i t y . h t m l   ˆ      W¦    #    @Æ [