|mkxกฆ}x}eฐo|{p}s}|yxuy~}}rel~mขig~siฃค~wyrฌpltz{{wค}Wgข{y~}gzkฌ}x|}wjฑwywmwuYyqnsyขtyxsn{vrr~ธฟ|}qyp|wr yxzv~~||ขw|ข|soszชงtw^kxmfwช|z}ix }yei{|~x~{ขvvnxrณชก{kv~zq{m~}qv q~wxqq|vvzฅihkzspsasyqlq|~yeyszv~~}snYqvv|xทIiqxsxฃค{dr wgyxzงr}pe~dtฐpwyz{ฃxxฃกญe~p`jqqฉxVฎฅ\|M~duw|syuyw{บrq\dCjvข` uVtฏzxก{~_du|^nqjชขgo~r_jณ[ข}ลft~}lฑOyฆ}{xทNkฉqiPgญPคฅจธcwnmOจญW[SrvW]HUe iiLuahwถfขro~vกฉxX[ewฆช_zmv[s^kงNp[fกOe`กqh}w{`ฅhtoข`dฯฅขtpฆFY[ณฐฌ}z`{}h|zฌypwฒvขกฏemu}]ฉ]ฃ|hpn}zฒxlฝ__]vxขu^กฅ}wDฎp{gพ\ผ}n_qzvฎ}gคswณzkกlกฌ[pta~น`kQzตCZ| จขfฆ_จรตjฉณqZฎฯmฐp uy{ap]x~oฉvxv|}{s|ฑดฆ\cpl|Yฎnบมquolcikฃq]}k{ฆxyixjxjmzwlc~`}xhzcฆS|n|kds~w}iUvez|zv[ฐ}tvOYxeOฉe}dxum]}ji rNc`ชvฤชnหy^aImฅww{{ธฌ sฟ~ฃkฟt^lmz]Yฌญwขuฉzฃthาeขmbi}Ye/mชฆmW[yนowฝด^~qXฏNwlbtฒrlจฆfvฃกTฆOฅ~ซz{ฤภ}llo}lชpฬlvoชกwx`mqช|bฑnชw smค]>ณฅmpฦ|}ด}H~zMswszฃกก ฆf}khsฐXtmjaฉฌกtdd}nrazpzf[p{ฆCtขธptพbnSฝขyqwกnดtlkrgsฃq\ขfowotหขopywiจIwlvx~ฃ]T]kUrtbjysYvฅJe{nmb~ฆ|rsuNfomnlฎ|wosj {|\ขq[grp|t{jwญypp]p~vZxw~|ขprip\svkxmxetb~{}OsPxฎkqr~fZyalkykkupธsnกxwkuprซuฅjjXฃexQsHaksaชฃV{qq~sjOp`osvกm~งrJwntณ`|jy]}uvr~~}ฌvtzop]q|rฃmEqKp}xfkavฆziNญtzcrtชyf[{}vhsd{xn\ovnzsly|งrขฃ~evwevs_lคsv{iฆu[U~hทrnq\adทq}r |{htry~ypln~Wz`meVyrนpyrkธ9d|uxช {`vxdv|vdi{mEiEq~จjjivueNmFฉXผycrIwkhQ^y|jrnฃ[omฎspqya~bna_u{oฌwyฉt{ฏleeuqr^rum|pvvvqzlvy~dlj~}Znp~r~Jjคqobwyชxqrxง_mjgluซh~zxlY_vw|oญ}ju~ฆz\ivt _ojYtwk~Voxpvjm\kvog~งฃกwsscsfตx``jwถvง~rZฎ{qmi^ซ~sก\Mmฅpq|q}ihwqzณTsv|uo~jdx[`y_|^W~ฅงaykx~unzqฅ~~{heaau{xXdn~euh|Wycฟtdibดu~yiqจZmZte]uzvก|uauzeงz{}lnหyxostr[ Yปwitz|c~iq}K~vlk `ue~~mup{q{zus^xstf[vGyf ฆz}}z`}จs{{_qohjกt|trELwvชupN`P]tt|~gfฒ|ชYgo}{||ygw||dคj^ทฆuxv}_tobฆง{e~fฟm{oetกxuฃy|ขeญyu|fxi}qzwซytns{j|kฑ|wกฉvqงzvnt|wคvj|กrPlv{ugeฆx{}{pnir{~ถfดงrpyniz~ฌv`tuจxฎd~ฎฎ[X}จsqฆปฟqzq{rlnq{u_| }hจtyข}ฐxt}_iuฃlyh|~|}v}uชt{qkl~ytyqwrฃ pq`\{ญv|{ luxneppxmo`sฆrzuq`ฅ||h{fลpx~ฃzpm}fYWzy~ชzsx|ruqjzk|ชกขzซ~zsข {oxIyซxgsvgzlio{^~ol} ฌfzguvi}ฅfn}จค~คw~|oxญxkฑelTvกven~rกqk|~ฑx pNVณลyYwcขt}oovyn~aQsซqฟ}xงxฉ]yqqq{{ypgtllrd{{wl~lx{ae{ysคWz}mzyzp}teชntxeลช i\o{to~rqrvy]ktคtฆelylฎx{~{swObject)
                {
                    potentialRootObjects = ((GameObject)selection).GetComponents<Component>();
                }
                else if (selection is Component)
                {
                    potentialRootObjects = ((Component)selection).GetComponents<Component>();
                }
                else
                {
                    potentialRootObjects = selection.Yield();
                }
            }

            var newRoot = potentialRootObjects.NotUnityNull()
                .OfType<IGraphRoot>()
                .FirstOrDefault();

            if (newRoot != null)
            {
                var previousRoot = reference?.root;
                var previousRootGameObject = (previousRoot as Component)?.gameObject;
                var newRootGameObject = (newRoot as Component)?.gameObject;

                // Tired of rewriting this so I'm making it crystal clear in naming
                var rootWasEmpty = previousRoot == null;
                var rootIsMacro = newRoot is IMacro;
                var rootIsMachine = newRoot is IMachine;
                var rootChanged = previousRoot != newRoot;
                var rootGameObjectChanged = !UnityObjectUtility.TrulyEqual(previousRootGameObject, newRootGameObject);

                if (rootWasEmpty || (!IsOnHierarchyChange && (rootChanged && (rootIsMacro || (rootIsMachine && rootGameObjectChanged)))))
                {
                    var newRef = GraphReference.New(newRoot, false);
                    if (newRef != null && !newRef.isValid)
                        reference = null;
                    else
                        reference = newRef;
                }
            }
            else if (BoltCore.Configuration.clearGraphSelection)
            {
                Clear();
            }
        }

        #endregion


        #region Context Shortcuts

        private ICanvas canvas => context.canvas;

        private IGraph graph => context.graph;

        #endregion


        #region Active

        private static readonly HashSet<GraphWindow> _tabs = new HashSet<GraphWindow>();

        public static IEnumerable<GraphWindow> tabs => _tabs;

        public static HashSet<GraphWindow> tabsNoAlloc => _tabs;

        public bool isActive { get; private set; }

        private static GraphWindow _active;

        public static GraphWindow active
        {
            get => _active;
            set
            {
                if (value == _active)
                {
                    return;
                }

                _active = value;

                CheckForActiveContextChange();
            }
        }

        public static GraphReference activeReference
        {
            get => active?.reference;
            set
            {
                if (active != null)
                {
                    active.reference = value;
                }
            }
        }

        private static IGraphContext lastActiveContext;

        public static IGraphContext activeContext => active?.context;

        public static event Action<IGraphContext> activeContextChanged;

        private static void CheckForActiveContextChange()
        {
            if (activeContext == lastActiveContext)
            {
                return;
            }

            lastActiveContext = activeContext;

            activeContextChanged?.Invoke(activeContext);
        }

        #endregion


        #region Lifecycle

        private readonly FrameLimiterUtility _frameLimiter = new FrameLimiterUtility(60);

        private void _OnSelectionChange()
        {
            Validate();
            MatchSelection();
        }

        private void _OnProjectChange()
        {
            Validate();
            MatchSelection();
            context?.DescribeAndAnalyze();
        }

        private void _OnHierarchyChange()
        {
            Validate();
            MatchSelection(true);
            context?.DescribeAndAnalyze();
        }

        private void _OnModeChange()
        {
            SetReference(referenceData?.ToReference(false), false);
            Validate();
            MatchSelection();
        }

        private void _OnUndoRedo()
        {
            Validate();
            MatchSelection();
            context?.DescribeAndAnalyze();

            // The set of element can change in undo/redo, so we
            // need to invalidate the widget collections instantly
            context?.canvas.CacheWidgetCollections();

            // Because the reserialized elements won't have the same references
            // at all, we need to remove them from the selection too
            context?.selection.Clear();
        }

        private void OnEnable()
        {
            _tabs.Add(this);

            // Manual handlers have to be used over magic methods because
            // magic methods don't get triggered when the window is out of focus
            EditorApplicationUtility.onSelectionChange += _OnSelectionChange;
            EditorApplicationUtility.onProjectChange += _OnProjectChange;
            EditorApplicationUtility.onHierarchyChange += _OnHierarchyChange;
            EditorApplicationUtility.onUndoRedo += _OnUndoRedo;
            EditorApplicationUtility.onModeChange += _OnModeChange;

            PluginContainer.delayCall += () =>
            {
                VSUsageUtility.isVisualScriptingUsed = true;

                titleContent = new GUIContent(defaultTitle, BoltCore.Icons.window?[IconSize.Small]);

                try
                {
                    reference = referenceData?.ToReference(false);
                }
                catch (ExitGUIException) { }

                Validate();
                MatchSelection();

                ValidateReloadScriptSettings();
            };
        }

        private void ValidateReloadScriptSettings()
        {
            bool checkReloadScriptSettings = !EditorPrefs.GetBool("DoNotCheckReloadScriptSettings");

            if (EditorApplicationUtility.WantsScriptChangesDuringPlay() && checkReloadScriptSettings)
            {
                bool result = EditorUtility.DisplayDialog("Warning", "Your Unity preferences are set to reload scripts during play mode." +
                                                                                "\nThis causes instability in Visual Scripting plugins." +
                                                                                "\nPlease use: Preferences > General > Script Changes While Playing > Stop Playing and Recompile." +
                                                                                "\nWould you like to change it now?",
                                                                                "Change now", "I will change later");

                if (result)
                {
                    EditorPrefs.SetInt("ScriptCompilationDuringPlay", 2);
                }
                else
                {
                    EditorPrefs.SetBool("DoNotCheckReloadScriptSettings", true);
                }
            }
        }

        private void OnFocus()
        {
            active = this;
            isActive = true;
        }

        private void OnLostFocus()
        {
            isActive = false;
        }

        private void FixActive()
        {
            // Needed for Shift+Space support
            // Unity probably changes the underlying instance without sending OnEnable/OnDisable

            if (isActive)
            {
                active = this;
            }
        }

        private void OnContextChange()
        {
            context?.DescribeAndAnalyze();

            titleContent = new GUIContent(context?.windowTitle ?? defaultTitle, BoltCore.Icons.window?[IconSize.Small]);

            if (context != null && context.isPrefabInstance)
            {
                var prefabGraphPointer = GraphReference.New((IGraphRoot)reference.rootObject.GetPrefabDefinition(), true);
                context.graph.pan = prefabGraphPointer.graph.pan;
                context.graph.zoom = prefabGraphPointer.graph.zoom;
            }

            if (context != null)
            {
                sidebars.Feed(context.sidebarPanels);
            }
        }

        private void OnDisable()
        {
            _tabs.Remove(this);

            if (isActive)
            {
                active = null;
                isActive = false;
            }

            EditorApplicationUtility.onSelectionChange -= _OnSelectionChange;
            EditorApplicationUtility.onProjectChange -= _OnProjectChange;
            EditorApplicationUtility.onHierarchyChange -= _OnHierarchyChange;
            EditorApplicationUtility.onUndoRedo -= _OnUndoRedo;
            EditorApplicationUtility.onModeChange -= _OnModeChange;
        }

        protected override void Update()
        {
            // If the Unity application is minimized or not focused, we shouldn't need to update our graph rendering
            if (!InternalEditorUtility.isApplicationActive) return;

            // Limiting our render frame-rate
            if (!_frameLimiter.IsWithinFPSLimit()) return;

            base.Update();

            FixActive();

            Validate();

            context?.canvas.Update();

            Repaint();
        }

        private Vector2 m_TabOffset;
        private Vector2 m_Scroll;

        protected override void OnGUI()
        {
            base.OnGUI();

            FixActive();

            if (BoltCore.instance == null || EditorApplication.isCompiling)
            {
                LudiqGUI.CenterLoader();

                return;
            }

            if (PluginContainer.anyVersionMismatch)
            {
                LudiqGUI.BeginVertical();
                LudiqGUI.FlexibleSpace();
                LudiqGUI.BeginHorizontal();
                LudiqGUI.FlexibleSpace();
                LudiqGUI.VersionMismatchShieldLayout();
                LudiqGUI.FlexibleSpace();
                LudiqGUI.EndHorizontal();
                LudiqGUI.FlexibleSpace();
                LudiqGUI.EndHorizontal();

                return;
            }

            Validate();

            // We always fetch the control IDs of the canvas first,
            // to make sure they are consistent no matter what gets draw
            // before or after it (for example the left sidebar).
            if (context != null)
            {
                canvas.window = this;
                canvas.RegisterControls();
            }

            LudiqGUI.BeginHorizontal();

            LudiqGUI.BeginVertical();

            if (reference != null && reference.isChild && e.type == EventType.KeyDown && e.keyCode == KeyCode.PageUp)
            {
                reference = reference.ParentReference(true);
            }

            if (context != null)
            {
                LudiqGUI.BeginHorizontal(LudiqStyles.toolbarBackground);

                if (!LudiqGUIUtility.newSkin)
                {
                    LudiqGUI.Space(-6);
                }

                // Lock/Graph/Variables Buttons

                EditorGUI.BeginChangeCheck();

                locked = GUILayout.Toggle(locked, GraphGUI.Styles.lockIcon, LudiqStyles.toolbarButton);

                if (showSidebars)
                {
                    graphInspectorEnabled = GUILayout.Toggle(graphInspectorEnabled, BoltCore.Icons.inspectorWindow?[IconSize.Small], LudiqStyles.toolbarButton);
                    variablesInspectorEnabled = GUILayout.Toggle(variablesInspectorEnabled, BoltCore.Icons.variablesWindow?[IconSize.Small], LudiqStyles.toolbarButton);

                    ToggleInspector<GraphInspectorPanel>(!graphInspectorEnabled);
                    ToggleInspector<VariablesPanel>(!variablesInspectorEnabled);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    MatchSelection();
                }

                LudiqGUI.Space(6);

                // Breadcrumbs

                foreach (var breadcrumb in reference.GetBreadcrumbs())
                {
                    var title = breadcrumb.parent.Description().ToGUIContent(IconSize.Small);
                    title.text = " " + title.text;
                    var style = breadcrumb.isRoot ? LudiqStyles.toolbarBreadcrumbRoot : LudiqStyles.toolbarBreadcrumb;
                    var isCurrent = breadcrumb == reference;

                    if (GUILayout.Toggle(isCurrent, title, style) && !isCurrent)
                    {
                        reference = breadcrumb;
                    }
                }

                LudiqGUI.Space(10);

                GUILayout.Label("Zoom", LudiqStyles.toolbarLabel);
                context.graph.zoom = GUILayout.HorizontalSlider(context.graph.zoom, GraphGUI.MinZoom, GraphGUI.MaxZoom, GUILayout.Width(100));
                GUILayout.Label(context.graph.zoom.ToString("0.#") + "x", LudiqStyles.toolbarLabel);
                LudiqGUI.FlexibleSpace();

                // Clear Errors

                var debugData = reference.debugData;

                var erroredElementsDebugData = ListPool<IGraphElementDebugData>.New();

                foreach (var elementDebugData in debugData.elementsData)
                {
                    if (elementDebugData.runtimeException != null)
                    {
                        erroredElementsDebugData.Add(elementDebugData);
                    }
                }

                if (erroredElementsDebugData.Count > 0 && GUILayout.Button("Clear Errors", LudiqStyles.toolbarButton))
                {
                    foreach (var erroredElementDebugData in erroredElementsDebugData)
                    {
                        erroredElementDebugData.runtimeException = null;
                    }
                }

                erroredElementsDebugData.Free();

                // Custom Toolbar

                context.BeginEdit();
                context.canvas.OnToolbarGUI();
                context.EndEdit();

                if (!LudiqGUIUtility.newSkin)
                {
                    LudiqGUI.Space(-6);
                }

                LudiqGUI.EndHorizontal();

                if (e.keyCode == KeyCode.Tab)
                {
                    HotkeyUsageAnalytics.HotkeyUsed(HotkeyUsageAnalytics.Hotkey.Tab);
                }
            }

            LudiqGUI.BeginHorizontal();

            if (showSidebars)
            {
                sidebars.left.DrawLayout();
            }

            var canvasContainer = new Rect(GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandHeight(true)));

            if (showSidebars)
            {
                sidebars.right.DrawLayout();
            }

            LudiqGUI.EndHorizontal();

            LudiqGUI.EndVertical();

            // The Unity doc says Layout is sent once before all events and Repaint once after all events.
            // From my observation, this is false:
            // - Layout is sent before each event
            // - Repaint is sent once before all events
            // Therefore the logical candidate for BeforeFrame seems to be Layout
            // Hopefully this is a cross-platform consistency
            if (e.type == EventType.Repaint)
            {
                if (context != null)
                {
                    context.BeginEdit();
                    canvas.BeforeFrame();
                    context.EndEdit();
                }
            }

            // Skip layouting, which is called once per frame.
            // This removes the ability to use GUILayout,
            // but doubles the performance.
            if (e.type != EventType.Layout && !e.ShouldSkip())
            {
                BeginDisabledGroup();
                GraphGUI.DrawBackground(canvasContainer);
                EndDisabledGroup();

                if (BoltCore.Configuration.disablePlaymodeTint)
                {
                    GUI.color = Color.white;
                }

                if (context != null)
                {
                    // Draw the graph here:
                    // The strategy here is to zoom out, then clip.
                    //
                    // This may sound counter-intuitive, but clipping before zooming
                    // out will cause pixels outside the zoomed out equivalent of the
                    // window viewport to get clipped, because GUIClip seems to ignore
                    // zoom values when doing its calculations.
                    //
                    // To succeed then, we need to create a clipping area that is
                    // scaled up according to the zoom factor, but then also moved down
                    // from the top of the graph window by the tab offset, also multiplied
                    // by the zoom factor.

                    var unclippedWindow = false;

                    if (LudiqGUIUtility.clipDepth > 0)
                    {
                        // Store the start of the window (inside the tab, top-left) in screen coordinates
                        var originInsideTab = GUIUtility.GUIToScreenPoint(Vector2.zero);

                        GUI.EndClip(); // Break out of the default window clip (the tab contents)

                        // Determine the offset that the tab represented
                        var originOutsideTab = GUIUtility.GUIToScreenPoint(Vector2.zero);
                        m_TabOffset = originInsideTab - originOutsideTab;
                        unclippedWindow = true;
                    }
                    else
                    {
                        m_TabOffset = Vector2.zero;
                    }

                    // Update the pan and zoom values with tweening if need be
                    canvas.UpdateViewport();

                    // Calculate a position that respects the default window clip, but extends according to the zoom factor.
                    // This position is in a weird in-between "offset canvas-space" or "zoomed window-space".
                    var canvasArea = canvasContainer;
                    canvasArea.size /= canvas.zoom;
                    canvasArea.position /= canvas.zoom;
                    canvasArea.position += m_TabOffset / canvas.zoom;

                    // Calculate the canvas' viewport
                    var size = canvasArea.size;
                    m_Scroll = canvas.pan - size / 2;
                    canvas.viewport = new Rect(m_Scroll, size);

                    // Make the scroll pixel perfect to avoid blurriness
                    m_Scroll = m_Scroll.PixelPerfect();

                    using (LudiqGUI.matrix.Override(Matrix4x4.Scale(canvas.zoom * Vector3.one)))
                    {
                        if (BoltCore.Configuration.showGrid)
                        {
                            GUI.BeginClip(canvasArea);
                            {
                                BeginDisabledGroup();
                                GraphGUI.DrawGrid(m_Scroll, new Rect(Vector2.zero, canvasArea.size), canvas.zoom);
                                EndDisabledGroup();
                            }
                            GUI.EndClip();
                        }

                        GUI.BeginClip(canvasArea, -m_Scroll, Vector2.zero, false);
                        {
                            context.BeginEdit();
                            canvas.OnGUI();
                            context.EndEdit();
                        }
                        GUI.EndClip();
                    }

                    foreach (var rect in m_RectList)
                    {
                        Rect r = rect.rect;
                        r.position += canvasArea.position * canvas.zoom;
                        EditorGUIUtility.AddCursorRect(r, rect.cursor);
                    }

                    m_RectList.Clear();

                    // Show a warning if we're editing a prefab instance
                    if (context.isPrefabInstance)
                    {
                        DrawPrefabInstanceWarning();
                    }

                    if (unclippedWindow)
                    {
                        // Restore the window stack
                        GUI.BeginClip(new Rect(m_TabOffset, new Vector2(Screen.width, Screen.height)));
                    }

                    // Delayed calls are useful for any code that requires GUIClip.Unclip,
                    // because unclip fails to work with altered GUI matrices like zoom.
                    // https://fogbugz.unity3d.com/default.asp?883652_e64gesk95q8c840s
                    lock (canvas.delayedCalls)
                    {
                        context.BeginEdit();

                        while (canvas.delayedCalls.Count > 0)
                        {
                            canvas.delayedCalls.Dequeue()?.Invoke();
                        }

                        context.EndEdit();
                    }

                    canvas.window = null;
                }
                else
                {
                    // Draw an empty grid if no graph is selected
                    if (BoltCore.Configuration.showGrid)
                    {
                        BeginDisabledGroup();
                        GraphGUI.DrawGrid(Vector2.zero, canvasContainer);
                        EndDisabledGroup();
                    }
                }
            }

            LudiqGUI.EndHorizontal();
        }

        struct CursorRect
        {
            public Rect rect;
            public MouseCursor cursor;
        }

        List<CursorRect> m_RectList = new List<CursorRect>();

        public void AddCursorRect(Rect rect, MouseCursor cursor)
        {
            rect.position -= m_Scroll;
            rect.position *= canvas.zoom;
            rect.size *= canvas.zoom;

            m_RectList.Add(new CursorRect() { rect = rect, cursor = cursor });
        }

        void ToggleInspector<T>(bool hidden) where T : ISidebarPanelContent
        {
            if (hidden)
            {
                sidebars.Remove<T>();
                return;
            }

            var content = context.sidebarPanels.First(p => p is T);
            if (content != null)
                sidebars.Feed(content);
        }

        #endregion


        #region Sidebars

        [Serialize]
        private Sidebars sidebars = new Sidebars();

        public bool showSidebars => context != null;

        #endregion


        #region Prefab Instance

        private const string disabledMessage = "Component graph editing is disabled on prefab instances.";

        private void BeginDisabledGroup()
        {
            EditorGUI.BeginDisabledGroup(context == null || context.isPrefabInstance);
        }

        private void EndDisabledGroup()
        {
            EditorGUI.EndDisabledGroup();
        }

        private void DrawPrefabInstanceWarning()
        {
            var warningPadding = 20;
            var warningWidth = 200;
            var warningHeight = LudiqGUIUtility.GetHelpBoxHeight(disabledMessage, MessageType.Warning, warningWidth);
            var buttonWidth = 120;
            var buttonHeight = 20;
            var spaceBetweenWarningAndButton = 5;

            var warningPosition = new Rect
                (
                warningPadding,
                position.height - warningHeight - buttonHeight - spaceBetweenWarningAndButton,
                warningWidth,
                warningHeight
                );

            var buttonPosition = new Rect
                (
                warningPadding,
                position.height - buttonHeight,
                buttonWidth,
                buttonHeight
                );

            EditorGUI.HelpBox(warningPosition, disabledMessage, MessageType.Warning);

            if (GUI.Button(buttonPosition, "Edit Prefab Graph"))
            {
                var prefabGraphPointer = GraphReference.New((IGraphRoot)reference.rootObject.GetPrefabDefinition(), true);
                prefabGraphPointer.graph.pan = context.graph.pan;
                prefabGraphPointer.graph.zoom = context.graph.zoom;
                Selection.activeObject = prefabGraphPointer.rootObject;

                // Same graph but different reference pointer so data remains the same.
                // So no need to call GUIUtility.ExitGUI() in this case when setting the reference
                // And by doing that we avoid ExitGUIException GUIClip stack popping
                SetReference(prefabGraphPointer, false);
            }
        }

        #endregion

        #region Analytics

        private readonly Action<IGraphElement> _onItemAddedAction = delegate (IGraphElement element)
        {
            try
            {
                var aid = element.GetAnalyticsIdentifier();
                if (aid is null)
                    return;

                NodeUsageAnalytics.NodeAdded(aid);
            }
            catch (Exception)
            {
                return;
            }
        };

        private readonly Action<IGraphElement> _onItemRemovedAction = delegate (IGraphElement element)
        {
            try
            {
                var aid = element.GetAnalyticsIdentifier();
                if (aid is null)
                    return;

                NodeUsageAnalytics.NodeRemoved(aid);
            }
            catch (Exception)
            {
                return;
            }
        };

        #endregion
    }
}
