// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unity.VisualScripting.ReorderableList
{
    /// <summary>
    /// Utility class for drawing reorderable lists.
    /// </summary>
    public static class ReorderableListGUI
    {
        static ReorderableListGUI()
        {
            DefaultListControl = new ReorderableListControl();

            // Duplicate default styles to prevent user scripts from interferring with
            // the default list control instance.
            DefaultListControl.ContainerStyle = new GUIStyle(ReorderableListStyles.Container);
            DefaultListControl.FooterButtonStyle = new GUIStyle(ReorderableListStyles.FooterButton);
            DefaultListControl.ItemButtonStyle = new GUIStyle(ReorderableListStyles.ItemButton);

            IndexOfChangedItem = -1;
        }

        /// <summary>
        /// Default list item height is 18 pixels.
        /// </summary>
        public const float DefaultItemHeight = 18;

        private static GUIContent s_Temp = new GUIContent();

        /// <summary>
        /// Gets or sets the zero-based index of the last item that was changed. A value of -1
        /// indicates that no item was changed by list.
        /// </summary>
        /// <remarks>
        ///     <para>This property should not be set when items are added or removed.</para>
        /// </remarks>
        public static int IndexOfChangedItem { get; internal set; }

        /// <summary>
        /// Gets the control ID of the list that is currently being drawn.
        /// </summary>
        public static int CurrentListControlID => ReorderableListControl.CurrentListControlID;

        /// <summary>
        /// Gets the position of the list control that is currently being drawn.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     The value of this property should be ignored for <see cref="EventType.Layout" />
        ///     type events when using reorderable list controls with automatic layout.
        ///     </para>
        /// </remarks>
        /// <see cref="CurrentItemTotalPosition" />
        public static Rect CurrentListPosition => ReorderableListControl.CurrentListPosition;

        /// <summary>
        /// Gets the zero-based index of the list item that is currently being drawn;
        /// or a value of -1 if no item is currently being drawn.
        /// </summary>
        public static int CurrentItemIndex => ReorderableListControl.CurrentItemIndex;

        /// <summary>
        /// Gets the total position of the list item that is currently being drawn.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     The value of this property should be ignored for <see cref="EventType.Layout" />
        ///     type events when using reorderable list controls with automatic layout.
        ///     </para>
        /// </remarks>
        /// <see cref="CurrentItemIndex" />
        /// <see cref="CurrentListPosition" />
        public static Rect CurrentItemTotalPosition => ReorderableListControl.CurrentItemTotalPosition;

        /// <summary>
        /// Gets the default list control implementation.
        /// </summary>
        private static ReorderableListControl DefaultListControl { get; set; }

        #region Basic Item Drawers

        /// <summary>
        /// Default list item drawer implementation.
        /// </summary>
        /// <remarks>
        ///     <para>Always presents the label "Item drawer not implemented.".</para>
        /// </remarks>
        /// <param name="position">Position to draw list item control(s).</param>
        /// <param name="item">Value of list item.</param>
        /// <returns>
        /// Unmodified value of list item.
        /// </returns>
        /// <typeparam name="T">Type of list item.</typeparam>
        public static T DefaultItemDrawer<T>(Rect position, T item)
        {
            GUI.Label(position, "Item drawer not implemented.");
            return item;
        }

        /// <summary>
        /// Draws text field allowing list items to be edited.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     Null values are automatically changed to empty strings since null
        ///     values cannot be edited using a text field.
        ///     </para>
        ///     <para>
        ///     Value of <c>GUI.changed</c> is set to <c>true</c> if value of item
        ///     is modified.
        ///     </para>
        /// </remarks>
        /// <param name="position">Position to draw list item control(s).</param>
        /// <param name="item">Value of list item.</param>
        /// <returns>
        /// Modified value of list item.
        /// </returns>
        public static string TextFieldItemDrawer(Rect position, string item)
        {
            if (item == null)
            {
                item = "";
                GUI.changed = true;
            }
            return EditorGUI.TextField(position, item);
        }

        #endregion

        #region Title Control

        /// <summary>
        /// Draw title control for list field.
        /// </summary>
        /// <remarks>
        ///     <para>When needed, should be shown immediately before list field.</para>
        /// </remarks>
        /// <example>
        ///     <code language="csharp"><![CDATA[
        /// ReorderableListGUI.Title(titleContent);
        /// ReorderableListGUI.ListField(list, DynamicListGU.TextFieldItemDrawer);
        /// ]]></code>
        ///     <code language="unityscript"><![CDATA[
        /// ReorderableListGUI.Title(titleContent);
        /// ReorderableListGUI.ListField(list, DynamicListGU.TextFieldItemDrawer);
        /// ]]></code>
        /// </example>
        /// <param name="title">Content for title control.</param>
        public static void Title(GUIContent title)
        {
            var position = GUILayoutUtility.GetRect(title, ReorderableListStyles.Title);
            Title(position, title);
            LudiqGUI.Space(-1);
        }

        /// <summary>
        /// Draw title control for list field.
        /// </summary>
        /// <remarks>
        ///     <para>When needed, should be shown immediately before list field.</para>
        /// </remarks>
        /// <example>
        ///     <code language="csharp"><![CDATA[
        /// ReorderableListGUI.Title("Your Title");
        /// ReorderableListGUI.ListField(list, DynamicListGU.TextFieldItemDrawer);
        /// ]]></code>
        ///     <code language="unityscript"><![CDATA[
        /// ReorderableListGUI.Title('Your Title');
        /// ReorderableListGUI.ListField(list, DynamicListGU.TextFieldItemDrawer);
        /// ]]></code>
        /// </example>
        /// <param name="title">Text for title control.</param>
        public static void Title(string title)
        {
            s_Temp.text = title;
            Title(s_Temp);
        }

        /// <summary>
        /// Draw title control for list field with absolute positioning.
        /// </summary>
        /// <param name="position">Position of control.</param>
        /// <param name="title">Content for title control.</param>
        public static void Title(Rect position, GUIContent title)
        {
            if (Event.current.type == EventType.Repaint)
            {
                ReorderableListStyles.Title.Draw(position, title, false, false, false, false);
            }
        }

        /// <summary>
        /// Draw title control for list field with absolute positioning.
        /// </summary>
        /// <param name="position">Position of control.</param>
        /// <param name="text">Text for title control.</param>
        public static void Title(Rect position, string text)
        {
            s_Temp.text = text;
            Title(position, s_Temp);
        }

        #endregion

        #region List<T> Control

        /// <summary>
        /// Draw list field control.
        /// </summary>
        /// <param name="list">The list which can be reordered.</param>
        /// <param name="drawItem">Callback to draw list item.</param>
        /// <param name="drawEmpty">Callback to draw custom content for empty list (optional).</param>
        /// <param name="itemHeight">Height of a single list item.</param>
        /// <param name="flags">Optional flags to pass into list field.</param>
        /// <typeparam name="T">Type of list item.</typeparam>
        private static void DoListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, float itemHeight, ReorderableListFlags flags)
        {
            var adaptor = new GenericListAdaptor<T>(list, drawItem, itemHeight);
            ReorderableListControl.DrawControlFromState(adaptor, drawEmpty, flags);
        }

        /// <summary>
        /// Draw list field control with absolute positioning.
        /// </summary>
        /// <param name="position">Position of control.</param>
        /// <param name="list">The list which can be reordered.</param>
        /// <param name="drawItem">Callback to draw list item.</param>
        /// <param name="drawEmpty">Callback to draw custom content for empty list (optional).</param>
        /// <param name="itemHeight">Height of a single list item.</param>
        /// <param name="flags">Optional flags to pass into list field.</param>
        /// <typeparam name="T">Type of list item.</typeparam>
        private static void DoListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty, float itemHeight, ReorderableListFlags flags)
        {
            var adaptor = new GenericListAdaptor<T>(list, drawItem, itemHeight);
            ReorderableListControl.DrawControlFromState(position, adaptor, drawEmpty, flags);
        }

        /// <inheritdoc
        ///     cref="DoListField{T}(IList{T}, ReorderableListControl.ItemDrawer{T}, ReorderableListControl.DrawEmpty, float, ReorderableListFlags)" />
        public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, float itemHeight, ReorderableListFlags flags)
        {
            DoListField(list, drawItem, drawEmpty, itemHeight, flags);
        }

        /// <inheritdoc
        ///     cref="DoListFieldAbsolute{T}(Rect, IList{T}, ReorderableListControl.ItemDrawer{T}, ReorderableListControl.DrawEmptyAbsolute, float, ReorderableListFlags)" />
        public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty, float itemHeight, ReorderableListFlags flags)
        {
            DoListFieldAbsolute(position, list, drawItem, drawEmpty, itemHeight, flags);
        }

        /// <inheritdoc
        ///     cref="DoListField{T}(IList{T}, ReorderableListControl.ItemDrawer{T}, ReorderableListControl.DrawEmpty, float, ReorderableListFlags)" />
        public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, float itemHeight)
        {
            DoListField(list, drawItem, drawEmpty, itemHeight, 0);
        }

        /// <inheritdoc
        ///     cref="DoListFieldAbsolute{T}(Rect, IList{T}, ReorderableListControl.ItemDrawer{T}, ReorderableListControl.DrawEmptyAbsolute, float, ReorderableListFlags)" />
        public static void ListFieldAbsolute<T>(Rect position, IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmptyAbsolute drawEmpty, float itemHeight)
        {
            DoListFieldAbsolute(position, list, drawItem, drawEmpty, itemHeight, 0);
        }

        /// <inheritdoc
        ///     cref="DoListField{T}(IList{T}, ReorderableListControl.ItemDrawer{T}, ReorderableListControl.DrawEmpty, float, ReorderableListFlags)" />
        public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, Reordec†xRnŠng]sh|a[oUŒm§|…k|p…w}m{~eVg‡€z^m|‚Lo…t‰txlh†{}††‡oc_xr]kyl‡³i™x _bn…±»nXm~}}Œluqey¡Š~{’}””{qc˜›|qˆanmZt‰r^‘lm^xŠ‹tySbhƒ\|vjyi|smtuo|PZ”€j„„_[§zpy{’y‚q€{Uv¡c•YV`’j_vwysˆœwŒ~t_\Rp`jizV`kt”[…„rfn{vltx}mx|fu„jvv”—p`dj€‹q„bqn\‚_c—…¢}v{|_nag{ŒuxiN{x‡op„Qk„Š˜†n’}^u}~€\4wph…}qiz~‘”mƒyyXl€‰t…sƒl}b~f_‹}u€tD”‚„™†q^¤ˆslƒšk‰“‚okŒ…sp„pŠx‘mazj‰hgl„rªpkjn~{zt‹m|€vutu_s}q†s€ˆpe}n‹yqy}ssv…ezf§Ygtwc…“…xxor€x{q`—kƒ€w]e‚mgt‚gt||{~ˆŒ…^ˆ€gn’‹¢toŠrvy‚^ˆr‚uƒ„Yl€|‹qnnfuuˆs{‚yj|v|jpthz_x‡}tuotswuv„Lc‚€{|YoRc}zzqm…y€~€thˆ{‡wŠ|€wwbqr•y—†c‚qqh‚v‡}†ron†kz„’|Œel£~|† ƒy\sokYoŠo\lbahƒp…zykZxl‡kˆsf|«su…xzlvdqq‹~txe‘‡|}—]|g|‚€t|€Œ¢lrqut‹wmŒ‹f}l|‡UY€grƒŠklsviwaˆy‚ˆpof”tcYqrŠMv{x‰s„rl‘€]Qor~sgr^„fnqmUiRk…n~ew‰}n…k‡wuKn}suJb’\}zxe“Š‰xr’ƒ[{y‰w€}}vftnwœgg…©sx„ZC¡gjsjyŒEˆstawwu‹cayouwyzh‡ Œ‡~a}qfkvtNOtŠ}x‰uc‘}n{{]€€Vzpil€sw}’ƒbsrlUkxqzšztphxƒhsbmƒ~rufwmmqvf†Ab†Uy……Ew^nˆ~€lw\nrvsltKvrvmdyl‰‰]t‰šcŠb…nc|yeoy¢aj{[Švg`ng|k^‘nrz{tgŒ“Sœt~Š|r‚„ho†grXyc€”nbzw}tuz[jŠjkolŒy™’wpo{y\}||u~I‰~hj|m‡U|~†d‡wk‹lƒ}a‚B„¨YrxˆfŸs{q‰aou€‹u†uis|ˆtn€kŠbdˆy~v„‰rromfˆ~swXx„ao{z™mk€Šwy[ƒ„s†oqOGy?f\hl~€d›noxQZ}k{dºjpd„zA…‰ci_xªc,^k]sgggh†mgpQs{Š Gp‹…sSovu]€¢zbhwn¤ZNµ‚\{xi l­¶€ph^qeªXfh€e•[u~U~lpI“s¥¢VxU‰uyœ}aOu«ik~d—†‘Zve‘X{vrheŠiY„GiL{dpzi.}lnt‰Ÿj^7œ[~5”‰PewO\tbJ{€©‘˜t¢†™·…Yo‹|^c}t‹fk|m^†}q|ÁPr™qÅ[y\U”c‹~kmŠ–‹b\œ„„r'¡{^a/xˆ\gg{®lT™_ltqœsw„¸w~vµ‚su·‚+XZyƒp™cY~§sn[hv{y•Q^€¥VYKk’•hvXŒ£Z€W^o˜]‘nx‚j|g<…¨£‹†n‡{¦“Ğhl¶zhW}^z½qz:r…pPÀ…\˜b[‡šlfd nƒ„C’‰}«u‡€p€o{si€zŸO}o€~qh|jp„m}yf‰luV{~x“r|ein…yŒ{cyf|muExvnsŒŠ€ƒXxyp‡osu}Z~}mI{?[ouczp‘r8Xu”eknoqi€b\dlbo‘theitkxp¦’ibr{vpˆhs†ljpj?‚vv}}ˆ¨b„ruY„y{ƒ{olg€“ZmfÁ¤Nu~Œs]nzxŠršmAsYtruŠg…~{ŠrMt_~”g]nOkd`ht}€ˆUš|jjsg‡ƒ\}x^dljd}/hmv}||}Uye¬wƒ‚_~v|¡pup„piÔxz~“es€rgvu~w}}]zcx•‡wŠror€npo…„ ]•‹o‡q`•[bŸ‰}ur}j‡ˆx€Šz€“pZk_nx_Zlyc“{…~nyv†rkwŒzO•”yw§pVƒ“•ˆnˆtr½†fy}nnŠo{„se~Wsi“j~{v…uqmm†ljlmnqp€wpˆ‡in‡mpjxwvg)…Š[w„e€wqj}{~†ˆfr{oldn€‡‰ƒqyvqÃjF†L~YŒ{Œp€qƒ”£¦oz‚q`‰oqxtruŒpgl[tˆ|nsŠus liwƒ‚_x~t­xxxywvœ}]u|x|tv~m¢wtt‚wljzkvˆˆ€v~•u‡y\„X•w|yy|…q„‚†ZY€ƒb¯jlb‹XD{rd€‚g‰‚€Ub‚h™Œ}‹¨wq„dtN©‚‡Vv‘Kft¬m‚{cQ•ƒrb‡_>•^Qyp_‚ˆuˆ€‰vypM]“rf}„‰†ƒy¥erE(Q€¤œ€uu—o©h…~^§gbˆ8owbw„œVi]†¨™V‰€q‘†\s†X”]^Xw‹fˆ]o~zb€n–uƒ¹xrvUW|pr[gp€@cˆ‡]vcŒ~v}goHb[{k…prrO]„`™v}eof¡„„mŸ–rt]–‹eS—Vv„]ƒs‡cŒgtlˆ^c[V‡ ³yŠrv‰OWQsPqyg‡Rs–¤bsr¼†k[`§Vn~mªu{^fnTPo‰{Wˆ €_tV`ulY`Ulq«KŠyFiŒ«~‹jvk’ir–xwHˆ\saŒ¤K„†Oh~flvŠh~Xg“yd‚—œ{vs5zˆIˆ=jl{Ya[ypŠ”†‘Mptp›t[lnzzd\†“zid‰‡†Wss}horisw‚i…`BWrrsmezzqzW‡‰~A}l~‚z”‚ju~ƒvp…\wn}fucj‡MydxtœYŒ|u˜zwyh~j“m|n—{ˆ……ˆtzh`{„n‰vfvŒ•w}qµv‚ity•zwmOguh„ni`j|uq‹uZsz|‚q†‚jk‹‹ŒUiSrv¨uzs…I~tu†Hˆ{yr„\cˆoo^…qzce…l†€†{€¨1ˆ‚lYtƒwVyhmnuŒ†Œ‰|}qqs‘™kj}†vuvm•m†Ÿw‰‡x†b”Uslspƒuhfpl€§‹mo‹€µ\mu—“‡Eixª~zonU‚`¡c|y“•|‰v‚S}arui]6z‹QOs„}tpisl[|z~\xxs„xyf‚‘|evˆzpq„wt†‚f…€x…Šqs‰Štxup†d‰|wcm‚b•ˆD†p~€}pfv‡SxZv|gŒo|eez„b±w†lWi~•€}tzJ}`„‘e’q†}zgc|n]gpŒprwxyv{jl™sfrmsj‡…P{‡xtrm‘{G–lŠÓ~’…y–”x„«j8ŠXtq™arlbp}Šlrnswi“]RDdmy˜faQ‡i“…~oŒd‰oŒ©©x§‘w€Šv~u†mwe‰jv¦dUiw‰xkqgh€v“ut:JŒ‹tSls‹qv~vv{‹†}zcc`‚††[mc†qlzWpŠƒŒfxq|O›‡‚tkfn?Y^…Še‚~•{dtSuu†zevœ[†z}\„pv/†„–sgy“{‹_„m‘nuo‰i¥vˆxxgf~tp_guPeˆsŠqWœ•f{—o}OƒYk^q“š\sPptg‰cr|}r~|llsŠ}}|EŠ€ohk®^|=^¦qg†_„‡}bˆt„ƒ`‹Œv”mwqs˜iƒ_q†Š|ƒitqg„‰€j`yut€e‚Šhz€xyh|~tx†DŒw—GYs‡r]’ktzƒlkUjˆ‚y‡ƒs9xvl–}|hZg|wŒunu€r‹„]iyjw…t‚hu‡–wtƒkg}m‰‚lnsgnmstiœey{chaflgziww{|TW‰czfsV†z~rŠ~yd|UNvf{n}q‚inr‘s`iySƒ|‘vrWu€‚]x~ƒˆuz‹Grwuplrpdn…†s€~|yjwwrn}}€y„uo~†xiw|nl~vqktw{}€|ysƒrk€€nzŠlnrmuiumƒuwrde}šxlczs‹…uo~qeul|cr‡…zg}qtyhty€r…qƒmxzy‡zyoowx{…rcouwyo„†}v|wqjzlztqu{{{zmˆ|~”v{q€gyywpkŒd}qqryu‚u{uuuj†nƒkv}yv}ymq|ppsptx…vwy‚zz€zomyjsprqout€b{vwq‚wzx‚|q~xtwwyqop}slk{wr‡€nv{lg{y‰†|~r|~z|n‚qylsww{|mo{{xv†km~q€zwuuusv~o|{‘mzuqq€mkzqmzvrrp€n}…oppvŒlmlwy…€xx€{r‡xzrxwe„r€„o|ps||k}nv‡€q~{{‹z}‡€rm„pW…‰~ov„wr”‚pw‚‚weUbYs‘‡oi‚bw`jow“„nƒŒoz{x’umqyg’‰zw¤}sukxxyƒ†wrgfd{yrq{„zlŠ‡sPgd}†tshwlˆ{lz~ˆheu‡jttzmˆpT„gkyzgv‚~|K…`c’Š~[if~ptn{mŒ|ph|jek„nWƒ…‡‰Œ|Ur‡vƒo}~wvuTtWv…n|…zqwvw‹¢jk{‰klƒwo‡yoˆzRtˆ‡Z†{}€ru—„mo‡\dlU‰agi€\usc†luxkugpbjŠ†kˆ~vi[šXmƒ]ysacx—t–}yblaw‘z¢„wbŠ‹‹v‡h™€_}|o‚u‰mmuo‹Œz{_Emv~~¢xƒ]`¢uaZ<ylgr}md„nkox^ƒ…sSm{”QirVgdwŒ€yx|op…}lwjfnwv”`˜rmi|†r‚jsqlvmvVzlsiuwoƒsŒgl„l~x‰}~g{dZhx‚}~”F}Pj~‰pY¯‹q|c~Šl’o™l†€loibfuœnjt~‹st€‚TEv{}or‹Mwijp|y€‡Mˆ‹tjf‘qˆd†¤h4k~rk‚Zb^yr‚€gƒ‚€oyOsel}€fjq~jl’ymauq’eZs||po}’tnqSvƒ}`k¡mt}xl…p‹†ƒY}„{xzy\n„|v\nl‹Ykj|‰uyqjvk‚}}wn~|„gvYa`Inw€€‡van`i~cpoŒyh’‰dh{~hrF€‘|v‚‚z{vs`zŠ¢wƒ|v‹mja“m}c‚ld€R}hoi²‰Ypxur{|yn‰Šhzx{“p€•n€tsL‹b~dxq€g‚lh|nump…‰ƒl:‚…€uKk|qrrzzSnnmt|zyi”cŠr~tmtsy†s“zrzns~m«‰sx‘’mk…vkk~€‘xVƒolxov•›„ul—sn„‡O›^u€smn‡vyr€Njyrlu{|rynt”švZoqopunW]x~k—A`VZxqy–ƒdhf†mzmœsi[„u^}xdmr’}aYƒ›t`sˆv’z‚fyuQ~eh’Š„V}wr€d{…l\t„ƒhmbSŒ{–‚_e{{w‚kplŠs‚O€™‡•~ƒzlTGš¡ˆ‰vi…a©]†ZL…m[i_„px¡¢k„_k”yig‰}mtjjÌ‰l}p†w{]eŒkguu™z‰€k”Ttm‡…q˜‹–¤Q€k‡J„ww”@v¿|mum`}ƒwm„„^Rstº²ˆ“ƒ–rb€œ‡“xPnŒ¨we„ŠgxTƒr““{Slpˆ…v°rxŠ…`Cq‰zÃtˆF…‚YšoŸj‚¡ aP‘FŠƒz’jq‚º‡t‹ywš†—„m^rnˆd…uƒtfxiq™tq„Thfu}bznjpi‹tn…szhzis{•X¡‹qvo€o}„“n„‡…‡b‡ssnN…ˆˆjd«~kqsyzŒsx„kx{p†”—m_kx–rf]hp•kk†i’Šr|p}lx|‚e[t‚}‰……stjˆ©z—jwohsh‡‰}mw}ourg„b™fVby‚vc~f‚“‰zˆu}z‚zr€lŒ‰‡kswkuj_zr‘všlK{‚~uuwueŠ |U†r{ycrŠq‘x~{€t€vg‚~pwv[yŒwŠ‰rwc‰u\qrk„Š„r{qw……cl`w}W}qjfaxkow{o|[ŠtfqbgˆpŠo}~x“e?ljh‹utvur‡x„ko€x|{wu‘w}‡q”jewevPz“|yqy`v~|j~rujh}\z‚zu{‚~†l‹„z}eŒe…†boˆqxnth}r}riˆ~_yfx~y`T{sƒ{jdj€‰]z“Vm€i—{~f‡}˜ˆ‚w„ƒk|uƒ\ˆ~zcp€Y‹„xo|yœvq‚Xku}h{…{qlnhkjqu‚icyyn„ƒwVhvqŒq|‰—~k{cˆ€|{™qkyqh¢Fxn†ss€leXqr`m‹®wk{wŒƒ€b‡hmy}ttl|‘‹yqdfkwm‚‰sŒnd•zt…[|rwdy†}vy›…}t{r~vLtnn]qu{xlˆ…e}ychqsjxoˆ}m„b„mzjt€t†unwzp†frY†}Œyl{i{q€ukjgv_ˆ{esmipy–y{snfrmpeymXs‚erœ_‰xk–[‡oŒˆ~sysˆxUek˜‘xhŒŒ‰gt€xo˜{‚|€˜|‡iŠ}{Xez\irdh]ƒt‰t~ˆ[zzbƒ”qn‡w„‡y}x……„…je…x}†fwvŒ‰[wcjqƒoyjZeq‹u}ruŠ|na†–„e˜}Œi‡u¡i†jutH„xŠ&]{sTsk|{buliu`^}k…{š|yšSk~oImŒ’fbR––S}~x“~z„vhv€`¤r{¦}m|d‰{pz7{wm‹Šlmrmq”œx“g“v“D]=Z|†‘t£x‚MŒr…‡ouXkultSaqh[‹>Mut~p‹d|…‰yOe“vzœykj‚gw[ytˆª“~{`“Š†}_y~{qys„c~swyU|„mnxŒˆpr£p€†yƒutt{bs_e|{{uƒ‰b™f“qzz˜iqfzfzru“Š|ylnšƒy„zh€wra™p‹†{inzjMt’Š|qfidshksw€eegEvjkkcaerŒr||mu{wpwvz‘tkƒzntw—n|Š—Nr]|zˆmx‚‰e}tkt{ˆ~‰}{yŠovebj–gpy„dƒ}o†Erz…s{Š~\§ƒxh‡˜ff„xˆy‘^re£‰fxVzozf~pLa†}~_€r}g‡m£{x^rpYy_}hfeqh~Zkwp©…sgŠ|†ikny˜uhsZ~z‡šii†}y|lr…‹wwnV…oxxšox~}‰amnxkƒlx^|\}‘_hvm†ca|`havEo:§j…}[ƒVyx~hwt•aa„`l˜uw€ sqdvfzl‘|wrk|t‚i`€wns}zjnnuŠ”ƒs…“pdivx€ijˆerƒdtn]oo~…|s†‚o„Šsz|sr„{sŒ}Uifliruo~{p‰‚™rcpX„{k’wsapk„dˆ_qwm‹„nsX‹{[s‡{ƒ…daogjxj}I…|ƒswoyc{ko€u{kc~i†uws…ZtŸsƒYh„n„‹pxv|}’weo}{usxzv†q[ƒ‰†lpbVu‰syfk€mj‡tskŠˆ_ g‹‘vmjq_‚‹`sp~{s—hS`agx€v…k{‰Yev€xn·‚Db{@–^†{|ql…t€i~m|s€~wmnmx?jjU…‰yv}poÆvt~œ„€zV|[pj~}lxoŠ|whŠ“uhs•rvsz}…~€uss¤?y“kw„w°‹R›_l‘Œ}u{{gŒk~v“pp‰i“—ƒxio€Š~yPv†qTkldsa‚x‚Ed„^zz{uvqhXyŸtƒxyjomyy‰tlƒb{w‚oqtp~pkWo‹{l{hSk{Œ{vayquxm€u‡€r^ytMœŒ{pf…x„}zŒ¥}r„^m‚i£¿GzyŒv„rfqpdu~u€ino{x\p`˜kŠmreƒl€giyglƒsyjnsƒ{^a}u{Xo~pmm‹xUvnhzo…e€•cv·h€qyk«J—Ujmrh–feŒxc“¥w\{qpT~bxktd}‡\j¢“s¦œ\]ƒm3ypn‡fqq‚ªehk^qlywxx’vi‹y–j’coPpU—fom|¨ˆœŠ{i[VƒRs^•‰mb’Xe{Ed’…~U_zZi…‚ˆ‰clg…]yuŠPš{ˆ q„u„hDrd²`…ai egpYw{…“[€uy€]z‹€‹LvHe~ƒ^{—†Œkm}]q~uts}pl\j{pww|{xtkutqƒšfsg‚yzjprc]Álhxpmgbª`~r}pkyuhp—“k€g€pirxzj}xŒT‹]‚i€mfg…kuhibhujª‹oŒ|{‹rv¥nn}™nŠ‡n‡B‡|‰ƒtZ‚cptoo~ig‹}yˆorg]a„…s‰“—lrvvzƒ|…ˆ‹}nxmtw›XZ„~|tƒ†ƒkwtg¥´}nuQjWd}ƒƒch€wŠ‰‹wv‰xf’ƒ”tj|zŒg‚{ikmr…O‰t•n€„kwl‚_l]wr‡zIQ›bxvfsoihwi‹\¢cs{uvlw¢`¶j…zrLŒtc|vj—‹F~xJnr‘Z}jx_U—U‡o˜sv–y‘bup‹‘skŒgo]ƒ‹€zeƒ_j‚†Š£^Œƒ~P€ts…rn´‚iyu£‹ƒnqs}ƒ~kV•~K…os‹gl`B[‚gw{rq]p6i‹q~{xqn„e˜mqœulqd„„s‡sƒ‡†Qu‰sdj\w€‚tˆ~€}z€vx€‰”“†‚?rKƒe[b‡gn…ˆu}poiÄeˆi€«t„\‰‹p€~Œ~z‚lgo wmrl›„Yl‰v•y`„b€s{|~‹”~e‚r™Wpƒ{xth{{V}P~zxXk\sv‘‰n‰ziœ}uhv†eyƒe‰€‰{†ˆeŸpŒ‘u‚zy†q{s‰}wz|‚‰xŠ‚””lib…©¤nŠTmb†l‡~mƒoŠup‰mŠ~ˆ‹ˆne€ˆ”‡n[“yƒ‡_Sis»\›‘yp}yrw}~„`…zŒ}\wh“}_fidldŒe~i{s„}•eys~fZ]lJgv\~ŠvY›|ps†£”Ko„|vd^_r…~i‘Eo{b{†‚b¤|tj{zjdsu¦\‚‰dsvy“Tt”ixy‰ibup˜nƒ\`€†©gtyfq‡e{Q}f}v~u9OUwa‰ntkv®mS2…“€tok„¥?uˆy`£‰wƒfxˆuˆ|•‹[f`nk‹y{’euZ|„‚£}¬vr_b’w`¬|Yzn|˜œ[ogs©zišƒmbgURlk‚ux¢ukadie—hi[“hcc|†`¹J††o\ŠoqnPÒ‰Š„Slˆa¸i–cUm•^knym¨€u}“p“›tr‡ˆYeXmv`gpu‹vwpsƒl…y]~t†i}wztƒktde~…†h…¡wvi€|>¦m|tˆ_xq†]”ontƒjKjWuoj€W}kq–Švs„n—qŒuo]|Œƒr’ezs…m}ta`„sr“‹mv—ƒvsr|^~jiŠpq~uƒh k`z}`¥nuv|ocŠk}w¢…s‚yhwGNp„¥†sfhtnxkˆˆƒyYz¥„„uY|pozkzvNopoj^p•xXo†ƒq€uozj›qyuiTbf‰sc’G|mbt|}‡d}jnozqu{nan•x‘Š…psqi‹|uf’i‰xu‘^~q…~‚[rufua‰r€{Š‰“{th‚–w‘l†{urgqtŠp—psY“\dWxhb~q8b_pppqUf“{s€~x„†‡n}_mt’ppš~‚]“vrv\€oqlw€trwp†xkW\_sdhzˆ“wpzŒvPvj_”£Wœq_‹‚lŸsA„k{—prc_\’^‰o`œ`kkf‚†`„bv„—”gsdp`TP}xXœtyI’vhU‹a‚XmŒzxjajwrvPKPZ[ob‹~†kqj‡‘˜gr~ˆ€x†ƒr“_€xEk†wxˆ}‘¦urjv€rww……‚uhƒvG|w€}vt>m¥jrŒt˜}Py”kqvšwge–³xw{pdt||hkpllŒws|v‰ƒRwdMQuˆs€x€Z~œ‚ƒ’ilTƒ}r€¸t„m‰Z]Yh‚‡pfjŒ_iL™bzfƒ‚ˆ¤‡yj~^n\„‡„Nq‰|vzxOUƒ^‹˜G„qŒ…xl ~daiŒk…—‚ …VœjU^y*j|~†j¸h„Ÿ“^lq„‰…²¤0^†~°^Î]‚etFœ‡h†by|{j•k‚…°n»l‘dhPUr]ysˆ‹œqbeu”¡`_m_Œ„…‡`J~€twiXy{o……‚ƒzwslzz€‹ƒvk“cr‚‘kuj}ƒƒx…qy|zŠn“k‡ƒu€c—yn|‘bi{pfw†lh{}–|er{‚‰pe€›mmmVp–{ln‚€Œ}†‹nl‰_myg‹ts†sr›{yukŒq{~y„wme`q‡se‰Oqbh~uˆ‘ikfztfdnrqw‹‚{z„w„…lry~~|Špllyfƒxuhyuƒ‹cshzz‰zdolpul„Mv“]‚u…o‡pcnvˆ‹s~hƒ~rsihm`tT~]vyrV|—““Š‹`T€<u„ƒ‡³{Ški{|n{–`“ˆyrrt‹d q€yŸ…j‹o„ƒ{‘nqˆey}Œbvwk_UŠvhnnguU‹iH‚Vngwƒvvkkgy„—œ¨Š[{x‚pzweYpqrq…ih|xXˆR|Zq˜w{m“q€rk|w’dbfŒ}a~tct‡vppƒoct†ƒ|ƒ’{z`rciokœ^w……™‰Šjv›cq‰|<•pu„‰ qwƒkYo@~q‹…d|ˆlS_ƒo„kz{fr|S€Qdk™x“z{wp•b—bj­uhŠœT¤dhj‰nU‡€˜‘’tV‰skv‡qŸv€n?t•q|c|ºwv‡x—b€‚Qˆqgq|lyu¢^snv‹srx–€wZ…¤…i‹Š‚t¥c†vwt€\ˆr£c\‘r‚‰†yumP|wnw’pqj^\–ptp›[d…|rˆxtZ‚˜„i†qshoo|lr€…c_ny‡w–€sœ_wlSc{‰††…[F[\F[onvr‡ei„k\aˆŠo”nxšˆsTlzy€œ“qv{œr~|ewŠzOi_q|Utœ‹u‘voyu‚hyws\{|s|s{psgk^mbn‚s‡vrwllvlnh†wem‘rc„sSŠ‹lŒˆpyh‡Šycq|Ff…]} ”˜Ÿ\_€jvZ‘gd‹rvslˆt—ey…rppcuq¦†ywmz~sqa«w‚Šrs€Quyj‹|{in‡zTs–|‰N\mn’|br|vŒL“Z_h}k…_vhƒW`up\}•‚¤o…€gytYkx{|š†koŠdy…vuUh›zkf‘[ol[fqmŒ‚|›q{dXkupZˆy`‚S¤Švqzkax“R„sf…Ãxv“ko‰xl`sjy}}v^ew‘q†s‰Q•}qs‘„kŠ–_t`gwm~rbSr{„”˜j‚r‡š[u–‚|d“c^…quxy®|vm‘f†‚j|z|‘‡€‹„~jbw{\Q^€n|ˆx‰oumŠ€‰ƒqu\‚o‰jnvkatcukƒ€oYz„‘cc‡__~“‡hmMqcnv‡†s~†joytwg‹nTjˆ•‡Œ˜i{}o–›bGdvuy‡qa{†u_{vyˆux˜ƒfulw„_„‹zšclnq|w‘~irz‹e…{yuhq¡mee{uzz~zg’€Xgj…nlpz|vtig}ucok‘…nŠ“ns„ppƒ€mˆhq`}us†„{‚m~Uˆy«šu|dt–Œ~jk‚lm{|Œvlr{‘†{b‘…——if…€“rŠnwjyz–v…lnhl~“„tw~[hpˆnh”Ä~_y€\‡dq‰z\€o|‡boƒvzŠsn—„z~]q“e]”ri_hhˆmju}csuafrphTkxxšr„“K˜”‰‘zjŠemyvŠ‚|~tX_“yWyk~VahCejƒ|…†KWÎ‚h\ƒoƒœzl›—…jp|qm„jwnsw€_’†H„[‰‡cXvfvOc†V˜jsnsbp…nm…omŠr‚x]nrƒswud‹pƒlublo„md‡‚zwŠ¢n€t‡}d‘p~…kix€h|Y†g›tbnmxy‹cvzl‰nvm„Qrvsmkym~lsxfugo‰|e~_¨j„ktyW]~cap…m€‡~~kuuvq„Y‚…|zˆt}‰s²|cr”ytzr_t}‚epi}iuy€Œvrˆ}|tP|fq_‰wglsgtƒ~yy„n‡v‘ydx|†y–mi‚x|„juw…x}yoxf}ssqsst~™x}‚g”€{€~{•‡|…g‚m{rmr‘ˆwbvs_VsWzy[row…zƒq’g„yh{wv}mz•yngY‡4‹rvqZ‡]‰yf}p†“xwhL ”eQ€sx¡‚„ˆƒkk~ƒ|¢kasX“lquntxpf^€j~MaoZw}f‚ywjpyOboqvzw„s„wegi‚t†qlarŠ~v€x~ƒtNm`ujw}p~iuˆsou†„`‡v||ŒsQ`ƒwsb]“y‘fwsrƒxbs[ˆ„^Švf[…¢xuˆmm|zb_†u˜mz~~˜ti‰N‡anz|‚s£Y|uGvwplty‡uq|x˜~g~s}~†|~‡w|m„ov‚zh{ksohry~ym~|bhŸ£•afŠ„fx”jtny…jypk‚‰p‚p£zl{w~ix‡ptvqqknwyU•pwt‚vYtrmpr‡m‰ƒ`s~~c‰{qq{_hyŠ‡ojml|}y}SloqŒ€sˆg`}}hshy¡ZŒ……’v„mrnpaqinzJcœ|pk„{e|‚¢‚‰i‘|wZ€z_pšxl|_nbf|`mjˆ–\dsŸsZ¥_fh–l‹ƒveƒw\‰^kli{jnws‹pzTe|u­†j†™B{nˆpl€gZk~Qer‹†\c‰=t”fsd\ƒ{xS™lzn…fkŒ‹tn}–ŒkpŠ‚qvbU…q}ˆ†Z~m{pŒ¤iPd˜w~tvXPd€{riuprŠxwf{dtr|n‹‚zur†sx†~vxpu‚pzw~‚ks}bjkifo…yxt‰pjˆyŠuw}ƒ‚zi‘ooš„€ŠuzqzˆŒ|yirzp~R‚t„lvqiW~z£m™h}™u€b—om„’q{‡yFh‡{Q|}o_w˜hd|iwigvq‚€›„•ft_y†nx‚} smƒkg€w‚nd–c‡…soQƒr‡{l|ƒhh„xzYyuhqˆzvƒizs„–`v™sn{}d{r~W~d{n~†lu‘myic‰„8kr†qz~€qs€pxK^s^}mpmxw{”qluq•lpdˆubyrijmexd‰v†ˆokŠ}wrt–d€™†e€…Idxih}‹™zˆqugj~hw{r«„r€mub{wkj|s{ƒlx|bylyzkUzu€cku‡}Šty`pYŒYu~t~y@c|ou}lahi’Ÿv[Œnq¢f¦p„t‚‹xVjeUiv—gw~­Y‹VmsTpGTŒo€rb…Vyr•{ÓhƒpzJ}R‰ŒfŒyXwkHa``šeyu¨bewœb†•E†|sdnI{|~†yk„bœf_ m_vfmŒtŒ‘{oYol’±”Zf“™oi‹{vuHzwd_ih€fio€`‡kl€vo‡YlnzwPGOJ]a†™^‚„hh–p†muš^¢Œ“”mossNzSd”i…n}]…›Y–‚r’R”…ƒ|—fWŠX^dT„D˜|~Xit…\Yml‡{q’ZŒw~fxSyny^T}‹jrh†m‡nÉs˜vi‚zrˆ†hj|xskhœv¢‘šUt†”hvmZ‚t—pŠ‹‰tUŠmnxyƒ…\ˆ}vuIŒwlx_‚yd„—xYxW†vovznp‡m‹l‡xO­‹p’Nq]|`ne‚pxbrUfxiSP{gluf{‚Œ€hlwƒsypnn…€Uvƒ{{†}upz}szrv„Š‚y‰Š~r˜k|g—{xiu}tuwmou„ƒlvŒ||st‹€q‚d„mfuxp|vgzƒ€srwk€‚zx}zxsq|us}|yŠvy~}u“zy‰oqsƒsuti}xomzy_‹€ƒƒ{quyw€k~nyipvxxlpt~ps–…suyyymmpxs~hmzqtvu{pvx{otsix‘i„‹du‚‰qdgksw‹~va‘‚Uqoj~vnzth`enqshf†vl|~‹l‹x‚o{’yƒ…kŸtn’pzv„‰t`wnh‹|ujt‹€q|{tiˆ{ov…tmjƒ{Œk€“sˆs~v~ys”Tx‹o}|uk…iw‹vyhk^unvvwN†lwsb†_r~…‰`|va…{v~kqu{zfZ„emyk˜…s{xz~_at†ct†‚~}‚‡f„jja…hq‹s€Su|™c„‰wu€movgn€[›Š^{m|…so|bu†rhq†j‘q€kvwyaxTtum|_sQxuz~~hsc‡‚lsouu^ŠroFg{€ur{€my„‚iƒ{qoƒ|œxƒ‡Yv`qNn|cc†Zi^txrsk‡oir‰g~rwkx|ƒvomk€~Ÿ†…tŒxXˆ…†M‡Œ’{j{ov‰amf‹sš‹z\}€ŒMu‚v‰rpZ–{œuv†ƒf|sm™yld¬on~o…ˆ}r[jsixa~Œj{iˆy{~pŒ›c^h„€puzvjwwƒ‡f‚ŒnuŠu‚~Jƒu~v{|‡_Wˆjryl‰‚¢……qxsv~uhwy|iM‚‹…ei`|„eo|Rm…[oxndolgqc{{yV‡zglitrlxCƒŸ„YƒPŒlnm…\s€{cnnyuyx¢r‡neh~~Œ{lp€qlq‰cŠzu~‚ˆ_lt~viuzsaqpt”zfƒhqZœ…c…wdy]sŠ€rt€yflktyurff|uwsyxYpxj‹„qŠusdyr…x|c~j~¢˜is‹Š™€hpd‰{ay„@edfƒv’“{y~ht„ˆ…xyw}‘|kŠxƒ”‰pŒW‚tm[vzoznƒ}Wcge‚]z†a—rƒziƒ‰bl~˜rg€iy…¯‡w_wyu{…wq{{’adpjxicuŠV„|†`{ˆusyv†\XxH‚w€h€n}\tf„qr{eŒ|‚u‹tvfvxgsxuxsh€y`xn€yka—xs}o„wruo{vus‘†bi_‚Š€d|rŒsvtqbL}aHgzsv†t_g…hnxyygd˜n}„rn\eszzi`l†„w[unq”w{ˆ`qny^hqnuu~ca•Rxlxwˆuu„—mlvlŠs·|}l„kwl†nr€ˆyw`rg`u{yui‡}‡qY~p†ƒrc`m†u•e‘Šucinj‘~Ševmu„{ˆˆn…wijtˆln^†sp~t|quuze|szwlc~}s…jz|l}ripgx‡j‰y†m„‹V^cxkl€…ylWf{€e€^‹w‚vwXbbhiiew}uhnx…†ˆ€ƒfqh|Urzpf‡œvW‰`qvxp„ƒh`z‡k‘nˆ†wwvWd}{‚—RnªI¿d¯ f3>¨Q¡antc‰~|-—N£gsZ÷ƒ„š…¬\€oL»N;„oaš7w–)Z€‚lhoeP‘­VW|ÄŠF-Õr¨¦]}Š¹|}p¦¼Tt€–™smK,Xªt|my“~Ÿ]{‘F˜}Hf^¤¦dHgb£R­¼G‡Mop|wx€jbDjxí¢c2‚«O'tae‰kŸ¹H¤ok‰¢G{•A t†s•_YS\˜x|gsƒ‡^Y³}Šv~›‹^ƒ‘“lp|ƒs‚y‹‰qqm[|{uk‚z€’{rœtm}Štobvz€…Y“^ˆcnW†ƒxz~vzi}|‹aƒ‚uq}Ã}u\Xb|szovxlq„n\z~ƒmtixƒ”‘ƒ}ewb†y†`ƒxlU®|“™pswy„xg{ŠjjR‚[ctkƒ‹ay|‚z|vjnji{}t}nt‡„eCœiƒvzŠzyi˜ˆ{„ww~|feqabmxpZ€lc¢pH|wx~–}“kxŒŠ}‚kb‰k]…]GOy†ƒx˜ˆ|Ÿ`}{hE˜{n•‰‰„rtvƒw…xlc†‡xselT}xx¢†jv†rk]zq…q”k’tuSŠTƒ~‰‚_Š„ir“hcy†oPhtV~‚uƒ“Šw”°•py]qc†khytmn|‚Œsypvn…|b‰v†Z‰mmu€of—tLŒuiz{šz¢|rny` c`z•irhiœ’œr€lewe…o§p^{kŠf›kt€e€SuW†kzRoˆa\ƒ\zo‚o€wf|zthm’J™sš‚’†€}{—cšŠ^fwTcvUoToo•lwj…Š‹x»—„•´loxwq„‚…kl„„ZqŸhˆ•JvTpW”|~Es[§ga}€u€\‰‚ˆ‡cwƒ€‹ykkFŠlrusœfm‹v|`WkƒjLe€|tnvk\ƒ”F}ˆŒrvl|i^’…wƒg|‚}ukorushsŒtwsnrhtum}‡n…~uzc›u{v`tx|„e…joxhqy‡pxxvwq€c…y~nuu—‹ƒsuzsrnohpxzu„|pzpxpx|wpz`{†r{{lVy|…okwrqx€cz€nwiip}}s~m‚€}l…y‹}…€wuwv{vu`km|s„}lg”|{ƒt€y{z…cp|uzx{z|q{uyzqhxrayr€r~zwswaxw€‰lh~vmozhmt„q}oyt‹o’t‡~tqn}nou‚{‚g„qvjƒu|‚rrpog`w‹{‰‰Š{‡™r‡ƒo{tunjxw€€fQ~|p{€…ep…„ˆ{z‚„¨zjp„†ks…n„w†vw}eˆmzm’gŠ‚yrˆ„_‹}‡nutz{‚t‰eŠz‚gƒ…|“‹o‰tqntƒtev“p}{„wmy‰sy„qtz}ˆfotG€o}`–•{uzv~‡jljM}pƒbt{…‡b†i†xƒd‡lgv‡ƒ}jvq8i§[|_|sd‚z‹u¤e‡k_prn`‰szz“za\gg†œ‘pi~vjY•Rd|trx’lW•]†bcftqqj{_pRdolnqfjdZry[ojs™kr‹‰•i~n~‹~˜Q|{ewtQ\a‰duwo{lp‚€}rnv•mW{l…eNe£ktŒotfˆ‡„ŠnŠtpK‚‰V‰¤ˆ|—¡uzn|nsj}fX{†}xhzz{Œ…~cvq{•dwwƒ^y{ubp„}‘Z~ˆq{vp^g…}yszulj—pa_‡`‡uj[zmWwi˜‡Eq€w[p‚fvqŠmup{ss”{¢~Šg¤wrn`‚kvm„azir[xxmŠ³‘cy}~€mtr{rƒxd[~¡ŠNz~†…Wx‘›‘…u˜ux^|izozw`›qn‹k†kL\ƒei}†cosudmxn~bxvxT{’|sv|ytnFw[‚y„s›F‚Wxrp{{i™xŒxƒj‰{mv{qtJ{c6pygswycŒ‰ˆ~py•ttcvt–„`ryŠpz›~„…ƒ‚z`tmktsª‹dwp‡•gr€v9œ‚`o–’ŒMykWal’Zx‹wqbvlTg€~~r’x”PN‚[MŸ{‰z†uˆ]PŒwX‡kk„‡‹u˜s„jq’z|Tc‰wm…pkksczƒxx…{k‚o†xi‰v…m‰r€`x†n€kj}q\}zp‡iarOh|jy‚vqw—Šc]b|jHl•d~S{Š}„‡imŒy‡puwšysn{t|joi]w}ƒcgza{†Rfx~uyst‡yœ~—ksq\powe’”ƒ“l^Œo‡tq„vu•jo‘ƒux~t’qrss‚“‡ry™_hƒyƒonu‡ˆ‚srzˆg{jke”s_sf€…Œix\zp©‰{jXcw‡ˆWe†~gzelySip{qyd_nŠ\y|{g†wšprk~[|fgxouwjW€o‹„‚‚ps~k‰|v‰‰‰Zz˜e_^o‹‰…Z€‡ol~nsum†yjkyˆcmUsoi…zlcUityn„Zc}{‹ˆlqz{‘|w_Rƒ~x‚‹¨zˆoaLil}|ilq‘„ofyTkrj€h}u‚pkuxŒvf…l|”…W‘}tNjcxh„|^ƒnzz{Vt‘z•P\ie’h[ql`Z’oz~x„|v{Et†rr•ƒsh[—liwbm^•Z¦|–e{ppo˜‡i~ƒhfnfskQ‘ƒ_‚~zVz}qd‹¥Š†mmy|UsQ…ex‰g|˜g~»„…l„Tš€ †z–t†pyt¥‡xRXt‘l„oš‡u“xWn~‚x’gy‘e‡rohgŸ}§Š‹M‹l€‚htu†aj«„}‚lqtƒ{‚}‡‰Grnwp™j{k{vqa`~wlk]ih–\‚qs•‡bktŠ{pj~l{†v‹ŒŒ{ry€gjgd}ˆ‡iy}wvvsƒzk€l§qƒsT‚r‰ovjkopi€~‰…ƒl§ewlŠyˆ|\‡’h~n€z{›Xyt{zqŠ[wcˆ}x~ugZxt„dwyz~szˆ‚sq…‰•T~vv…jw€ƒ‰pqqr}qp_bUusx„{iqmx€uoŒ}unno~yr{et{…wƒ|sowƒksnƒ~€trer‹oq~lsd~ssnqƒuu~din{mx{lwp~–zo€xn}i{}Œyluthˆjt^tl‰q_uw‚rsroqsq}x_„z‚ub‚r{uqlrwƒ~{„‡…ihglm{srtvr|oyuc}k€‹ˆlz„‚†fu…prŠk‚nkr|uu€`~s{z|j‡|‰{ynvewx„|urunk|Œtkrn}hytrvxtuoƒƒwn’€€~urxu{uƒttzxq|\…r†|…vq{u‹~{€v||€‘v…v{|qgsƒ|ux€|dt|r„‚z}u~u†zvƒmv[s|jzxym„ƒ~t{xy}†sjuvt›{wƒrpƒu~t‡|‚z{vxzq|t|gjt†s€}okƒy“‚spw„…€vkw{yrst}rvtur}r{p€g~tj‰nwb{zspxviqxqaoltw|n…vey†ƒj’|{{|mwykzrni}jyyƒz|hp…zf{xxr~ƒm{soko†yunu{qpcƒ„qhyr‹n€{wnŠu{ƒ}|zy}}Rroi~uw}ul‚pq{qn…uz{zsu|n‚xworqs|}zc€f}ymuwmrysq‡ys„ix~vvwzo‡vwvqy|yvmq„||ol€€’v~p`†|^uymT‹qzmwk_z{qia‚ƒ–bncrf€lbo€|f‹umzTastpzsebn~wk‚}u`g|b‚†n‡`lws~’ucnm|‚{…†‡€mrq€t‚px‰‚poƒdult…uo‡rw‚†}mt…o„‚~dt²lˆy„|„Q†‰yex…€€‚u‡xN‚‘Ev{|Wryt{Œhƒk€Œq€‹gXƒ{Ygku`‰u™kuo{{r¨ŒyƒolmpwŠSZvŒyp†uŒˆpƒªa‰fwu‡ƒŒqu†sX\€U„kX€‡tm|]\o‚p~ˆ‡{_o…_uWwxŒMy†}gŠr}†—™ˆZr–…‹˜{o{h†|z„qwyh‚‚†cgl|‡c{‰x~pou|cuguŠ`s}k„~mg€f[~„ƒ…hYyh`e–nq|<vhoaˆ_w{vuxtrocŠd|›wg~b_nŠxq˜ƒhˆ‡„Vrm‹~hq€m‚ €z†vV“\yŠhxu…_Šl…ŠJgm…\t•…„m |\xk{gw`uy…tz|h•s‚‚ƒ[m~k…mamŒƒˆoyˆ|wxOgv€tf~‚Ww‘to|asaYƒegyxspn„„•xZkšty€‚|ƒ{x†z†||jpbxx‹„uryk‚mv•v‰u‚vi}kp_ks‚m†tk„’vg€†h¢j€un®u‡]_zWx{m™_lƒ`‚xp“†rn›`…d‘’~‚œwpŒdyjao~™“x‰k†‚Šˆzwjjxnkƒ…Mr}qax„]gR‡•—n`l†{™•wNjbz`]kohpšfk…lbofws}v‡‰‰‡o’’Xxzm…u]vu[[‰ŸqƒŠk†>k ‚‡quˆ^tV†so€uv]Qx‘‚}ˆ†sXvv‰w`jw‚g—evk{‹|ZlrŠ|selœƒ•zn‚S~‚„†„€u•yWƒunw}~uoyuxjƒ{ˆ{‘\o†Qg}Œ‚k‰_}uˆ{p”_uYrv‚v†Œ¢‡y_pq{šnymŸhck”^~}Œykƒ¿}bƒx_“vs`›ixds|lN‹egk|sjjiZ“so”d}•~p‰Œq…zƒ€On<…^Œ}‡_˜pyv“f}p{fw…qs}t…yfy”¦oTnƒRndk}m‹‘{{{‰lj|plvpnhr|®alg“‘†yysrg|Œnx…€kŒ{nO…nˆ†i„f•l~gxWt|}y–wdtc^}”ciq{xn|wf|jx‡‹ouj„}qƒwKeˆ‡ˆumr–~|hwlpzˆ‚yioh‡‡€|zxy~[‚ndf©|wˆgƒnn–_‚|}€zfx€xm|b¢s||y„€~ƒ€ˆ†bsltjnzczh’h>‚Yjyt‡§…€‹ˆrsšqgsnkOˆft|y‘Œpi…c~{o[j“²z_rw}ºuvnov‚‰uihiftjYpNN…x†msZuŠ|m”]tm‡waw_JM}|ƒcœiuh’rp‹sy‡˜†y}ŠœXƒ]{h\woykbmM²ƒŒ|•‚e—|}u„gk~†Yd^Pnvlv€@|yfa„‘qb€wI|VFos\|{„wx~c‹_l‡o{pjW¡tˆh{{VXŒis’s’hyG–h{x}‰giŒ…‘n‹”~£lg{mq}mRwvsdcˆpv”~ˆ“ Id{iedƒÒ|p‚u}]t^VE~†fH†Mv:¡q~U`l„w…›{l‡_¸†kr’–x‡t†Rzsz€s[²yt|†Š{l~‚|ptdg`‹wilh}m\|S~qg»Œ‹\”{iˆnn|xv…o…v•{{p\„x`qv–k{wšŒƒ‹mšvm”“u’vhˆ‰nZŒusi~•afn}u_†Ï£…|q|rYˆvgi‹uBrpaš¨›hqpŸynfv”Šœ‘}š^˜z‰œ_†d~Šy…`D}—z”hUWx‹X‡‰¬ŒGt[™xn“µ|VËi\Wš3y¡oh ‰Z‚Kaf|o‹ƒC…[|„{uNh…pijms¡bVwFi’l’R‹[›ˆŸ®ypa~Z^ˆ–mrJu—§bV”lYK`hk[–kÅzKt|lš.’z}„Eb}f‰q `q‚ ™}yc\q€oapvc|pdZv{’V„kq£~~b“{¤u‚vivxŒ”š»mvw‰d„‡vhœh€‰ govs‘k“s„ƒ_ŠT{”h…ˆ~Dar\ƒ€wO€x†‹\„ÖrN™b…‡h]fFh§h€y£‘Šazm$Zfb‰p‘uOˆcyral}vx}~‘Y™¨[z{_pœwg˜NZO|y}]Zrfƒ_HŠYlk‚hrzAv\‘RWj|rx—{yoFWzšfo€kp€xrv‡wŠc³ƒonˆxUS^iVqˆ`t_RŠyyhSqy`{ˆ…ˆdos]fYSœewghizO€œ•j{uggi‰vc^‰z`ifq|vŠSu‹‰u‚qspˆ\jvXŠ‰†n‡zc`_>ow”wlbrmt}Œ‹zaogbwƒi|nƒp“jwqjjzh„‘pQšR‘wm‹n„htcY‰Xqa‹v€lu[}“‰{quqˆj`y‰†upShnŒ{wq“‹ueSze_gœ„ufy~„•s…Wz{Œ®r€TaZi’s}˜d”^Šv|r„xX^lNˆj}m€Zq§ptp†‚ov †ko]n–r^zºkŠp‰job[‘n†l‹j®dfxs§wVP€yvrd‰¥s`sš” {u|nko‘‹xqaie|tvlv|“uf¥g•…w}¤nXxk]~dh‹}g‹ƒgzfˆx…e]j¢~t‚w“}†kflˆ‚ƒŒkj‘vRm{ynv}‹ˆˆu|r}qƒf~o`N‹…\‚’iu€zjsƒ}hjip~`sw_lqf~z{y†\xhv”mlxkkyzy€ˆv„wvu^˜{€jeŒvif—]‹€|‚[†{|…rp‚‚_spr……kcŠ\“xw^…hzrs}`smk‚{yx{uw‡{xvugtth–…F•x`Šue€€Ÿp{iorxrfrsp„dxv|y\r‡o‰© z}‘ao]i‹”Šy…—Gx]’~uzx„l`Œ}U\Œxv–€g‡\a²ai|–‹tce|wfa|o‚ˆvgi¡Œqx…wˆ‹jƒ~|zƒuqŠ‚wopde{‰vczzOv\Yxv{~‰ˆ~rlUvogu‘|im†„{ssx…~‹c}‡Šx‰vjwvzpgqx[|…‚py{‚bui~ztg€Šzzjrmuq’hsyhu‘|•~}€t{Š~‰AŒ|sb€qfdVx’py\}•u‡[{‚Šˆo‚„Š^“u‘“„yyo}‚lŠiowxgq…uyuxj‘u‹Zp‰mm‡ps‹mŸ„]€hp}r{¤‚kvhiTn†…Š[fw_yd]‚b`€iO‘jf•Yz{`v„­odp˜qš^ar‡”vy€p}ƒY…Œpi„Œwxˆh‰wu“‰{e|€‰}Yq…hwt~q€„„zŠfvy‘†šll`}|dp{``h‡isljky_s¥Yhy`Šf~z‰eh‚rgxŠ}nc«ct€ywfhb…”old}cs€p”uw‡z”}‹ƒ–yu{{a‚ohz_„mWfƒi†”xpjttt„n‡ƒ|yl¤n}eY‹Rcssp…wzkq’£gŒ}Ššgxb€up~t•t`~wmy”b~ƒRkyk]ŸsNŠw€z|‡p†qtn]i„~q†Šm”gU}†iu™ttw«²aijŒh…fpƒ¬gzia`ƒ¦L’hjC‚z||‰~vÂ^dssš€y’‰{t‡t„€Špkw~Šlw”s …sinl{z……•”au\r‚u—ume_‚‘Hcnlow~p\^zŒftOs•™‘ƒqm„…x¤‰gPnfaUyuzu†iD„kš†{zl“^„nzg}{~\¢crv}feD‰’w uˆYLorˆ~oc{›†]nj|uz sxg‘bm€svrsŠp€yhp‡~ut{yƒƒbvpy{Xg‰btqzxo‹~g‰i„‚h—‚tks{n‡ozm…s‚hxm{—||{k_s^a€x‡u‚‡q‡_—†iv›‹emsl^›„o{plx€hneuj{po‹p{q†‚ƒyhdq˜`†}o„zci“™ctp…pmlz„nw†sTsu‰u‚s|xfg€­Š_q†ks•nmk~|j}hxw…uf{evƒq}€qpkZt«[}}Œ_`—–`•qbfipŠ‘u€‡abqhb{E\.¢°‚gpvyiŸjšb¬‚’ Bo¡dOcsowq™uYcqyc‚©nÁW‰R{~frXƒ†x¹F’ˆjox„xwŠQgXkbJh|s]lrf–|”•TvoŠ™”˜Uƒ¡†°^vfes\dD“U}c‰p˜ligšŠq¤Xs§|sˆfªPˆ‚w_fl‰ˆƒW_ˆœ‘o v|t‘pBC`–oyrN“if~qy{kq€j†xl„{›†€‰ŸTSioqZ¬Wpw«‹x‡mosˆeuw^_™Ymy‚_m›~xr†…‘uœŠ¿Y]…paWVwyzljVn‰‰†j|}w‡€ˆ€kŸvwgkƒ‰–c†„rw—}djj§rxo€j…ˆh”r™rfnctzg‹ƒŠ‡ppl€ecfnl°|¥ª–iyyŠ™mc…xuƒTb„gv‚jƒ]tT{gx­vqkrOni``†Šf|j_–ˆˆsueÁP®ƒ¥qp†ffuI‰¡wp|njZŒdzk~cm~hryxm…gs‹ˆƒsKoey–lŠyt]ˆ~nhisz}w…kvs†tt$ŠŒ–k{qe}~u…hv~y“nsxlspw€`t{n sT…z›ojVOo‘xŒlh^t}vqM‹•†‚mey’›u\sp‘z†c‚Rl‹vp’wq‹feyco^xjMm‚Œ}kŒzwa€vya{]wvŒ_‰zvubaqmqm msu†¯„lamb†~rp[„mkMw~{‘|{ŒeUkS}N]‡{˜…|NwV_€ys{†sx}BbxŠœ‚„‰csu„Šp‰twyxx\œg…mA›wugr—kZ‘_^xsr…~dx~ˆjyzbtro~†dqsiodq•ŒŒlWnn_qªa€ne”[cvcj?†y~‰|g¤‘tsq…‰™m%†“s™Åb¹‘†C¢~v‘ƒmc“tp`~]1w€g\VwkuXfE|µkF€jn¦;arExŒbf_|ˆ„zx¤„pkxƒfˆ{uqQYq|ƒy^d}„©W™Q\[¢‚wZ…™w_–‡}V£f~ x^)Oa?yl[T”mÀW û”FccJbZ^Œ]¥dJ…n¦v]„nœ‚‹JWii›j~pˆ][_|¨–g]TŸn¥‘œˆJ˜§ws|E¯dz[u¤um[lwhmŠxsptXt}gnuƒ{Š‘{pzz‡z‡€y‡•gr†|tƒ©wgˆ†ƒXooqgi‚¨kt’z†xxUŠf|a˜`;o_{ bnmfxoe•€d¢d‡ƒ‹|p“¤|o†s~mwm–z[f‡N\r˜^eu|vylrx”…u‹xmS_hx{rƒ€‡~tƒx_•ry…pnhn‡syÈqt\xuo}‘°Wnjt§ši“‡|os{os“|v„‡‚y~ccj…€pmplizv~‹xir~wbŠowqiydseŠ‚qm|gkxu~€qŒbnrv|]|si~otwxyt{’[‚qXrYƒ†evk›kŒt…ce{vty„in}zkx‘_}ˆqxon`…prriu|ˆy}–zTfhyuX]†Šƒy˜€ri€o…zd‘wsp|hxn|k‘t|ut„nl€^nœ}krpui“uwq‰lt„‚z‹}ll{zuzp…}ƒ{‹hxƒxq€~|slwt}wx‡v‰†…us†xdyvk‰z|}zmr“s‰‚„x{{†|vŒ{iƒovp}pz~‰zvv{phsnz„v‹}wv†m€‰{|\ku‡ujtqlxqƒo|p‰‚‚v€zjw{‹twtq„s}x€Šlyn€yy…€}†‡}~|~i}|‚h}ktqxyr•vk„ky~w|m|tvwupwŒŠ†nxz{}ixmƒknˆqŒqŒ}‚p†yq‚}}ƒrsˆ‹€d{nkvv…ywjqypcb{_v~zo