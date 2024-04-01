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
        public static void ListField<T>(IList<T> list, ReorderableListControl.ItemDrawer<T> drawItem, ReorderableListControl.DrawEmpty drawEmpty, Reorde�c�xRn�ng]sh|a�[oU�m��|�k|p�w}m{~eVg��z^�m|��Lo�t�txlh�{}���oc_x�r�]kyl��i��x�_bn���nXm~}}�luqey��~{�}���{qc��|q�anmZt��r^�lm^x���tySbh�\|�vjyi|smtuo|PZ��j��_[�zpy{�y�q�{Uv�c��YV�`�j_vwys��w�~t_\Rp`j�izV`kt��[���rfn{v�ltx}mx|fu�jvv��p`dj��q�bqn\�_c���}v{|�_na�g�{��ux��iN{x�op�Qk����n�}^u}~��\4wph�}qiz~��m�yyXl��t�s�l}b~f_�}u�tD�������q^��sl��k���ok��sp�p�x�mazj�hgl�r�pkjn~{zt�m|�v�utu_s}q�s��pe}n�yqy}ssv�ezf�Ygtwc���xxor�x�{q`��k��w]e�mgt�gt||{~���^��gn���to�rvy�^�r�u��Yl�|�q�nnfuu�s{��yj|v|jpthz_x�}tuotswuv�Lc��{|Y��oRc}zzqm�y�~�th�{��w�|�wwb�qr�y��c�qqh�v�}�ron�kz��|�el�~|���y\��sokYo�o\lbah�p�zykZxl�k��sf|�su�xzlvdqq��~txe��|}�]|g|��t|���lrq�ut�wm����f}l�|�UY�gr��klsviwa��y��pof�tcYqr�Mv{x�s��rl���]Qor~sgr^�fnqmUiRk��n~ew�}n�k��wuKn}suJb�\}zxe���xr��[{y�w�}}vftnw�gg��sx�ZC�gjsjy�E�stawwu�cayouwyzh� ��~a}�qfkvtNOt��}x�uc��}n{{]��V�zpil�sw�}��bsrlUkxqz�ztphx�h�sbm�~rufwmm�qvf�Ab��Uy��Ew^n��~�lw\nrvsltKv�rvmdyl��]t��c�b�nc|yeoy�aj{[�vg`ng|k^�nrz{tg��S�t~�|r���ho�grXyc���nbzw}tuz[j�jkol�y��w�po{y\}||u~�I��~hj|m�U|�~��d��wk�l�}a�B��Yrx�f�s{q���aou��u��uis|���tn�k�bd�y~v��rromf��~sw�Xx��a�o{z�mk��wy[��s�oqOGy?f\hl~�d�noxQZ}k{d�jpd�zA��ci_x�c,^k]sgggh�mgpQs{��Gp��sSovu]���zbhwn�ZN���\{�xi�l���ph^qe��Xf�h�e�[u~U~�lpI��s��VxU�uy��}aOu�ik~d���Zve�X�{vrhe��iY�GiL{dpzi.}lnt��j^7�[~5���PewO\tbJ{����t�����Yo�|�^c}t�fk|m^�}q|�Pr�q�[y\U�c�~km����b\���r'��{�^a/x�\gg{�lT�_ltq�sw��w~v��su��+XZy�p�cY~�sn[hv{y�Q^��VYKk��hvX��Z�W^o�]�nx�j|g<�����n��{���hl�zhW}^z�qz:r�pP��\�b[��lfd�n��C���}�u��p�o{si�z�O}o�~qh|jp�m}yf�lu�V{~x�r|�ein�y�{�c�yf|muExvns����Xxyp�osu}Z~}mI{�?[ouc�zp�r8Xu��eknoqi�b\dlbo�theit�kxp��ibr{vp�hs�l�jpj?�vv}}��b�ruY���y{�{olg��Zmf��Nu~�s]nzx�r�mAsYtru��g�~{��r�Mt_~�g]nOkd`ht}���U�|jjsg���\}�x^dljd}/hm�v�}||}U�ye�w���_~v�|�pup�pi�xz~�es�rgvu�~w}}]zcx��w�ror�npo�� ]���o�q`�[b��}ur}j��x��z���pZk_nx_Zlyc�{�~nyv�rkw�zO��yw�pV����n�tr��fy}nn�o{�se~Wsi�j~{v�uqmm�ljl�mnqp�wp��in�m��pjxwvg)��[w�e�wqj}{~��fr{oldn����qyvq�jF��L~Y�{�p�q����oz�q`�oqxtru�pgl[t�|ns�us�liw���_x~t��xxxywv�}]u|x|tv~m�wtt�wljzkv���v~�u��y\�X�w|yy|�q����ZY��b�jlb�XD{rd��g���Ub�h��}��wq�dtN����V�v�Kft��m�{cQ���rb�_>���^Qyp_��u���vypM]�r�f}����y��erE�(Q�����uu�o�h�~^�gb��8owbw��Vi]���V��q��\s�X�]^Xw�f�]o~zb�n�u��xrvUW|pr[gp���@c���]vc�~v}goHb[{k�prr�O]�`�v}eof���m��rt]��eS�Vv��]�s�c��gtl�^c[V����y�r�v�OWQsPqyg��Rs��bsr��k[`�Vn~m�u��{^fnTPo�{W���_tV`ulY`U�lq�K�yFi��~�jvk�ir�xwH�\sa��K��Oh~flv�h~Xg�yd����{vs5z�I�=jl{Ya[yp����Mptp�t[lnzzd\��zid���Wss}�horis�w�i�`BWrrsmezzqz�W��~�A}l~�z���ju~�vp�\wn}fucj�Mydxt�Y�|u�z�wyh~j�m|n�{����tzh`{�n�vfv��w}q�v�ity�zwmOguh�ni`j|uq�uZsz|�q��jk���UiSrv�uzs�I~tu�H�{y�r�\c�oo^�qzce�l���{��1��lYt��wVyhmnu����|}qqs��kj}�vuvm�m���w��x�b�Uslsp�uhf�pl����mo���\mu���Eix�~zonU�`�c|y��|�v�S}arui]6z�QOs�}tpi�sl[|z~\xxs�xyf��|ev�zpq�wt��f��x���qs��txup�d�|w�cm�b��D�p~�}pfv�SxZv�|g�o|eez��b�w�lWi~��}tzJ}`��e�q�}zgc|n]gp�prwxyv{jl�sfrmsj��P{�xtrm�{G�l��~��y��x��j8�Xtq�arlbp}�lrns�w�i�]RDdm�y�faQ�i���~o�d�o���x��w���v~u��mwe�jv��dUiw�x�kqgh�v��ut:J���tSl��s�qv~�vv{���}zcc`����[mc�qlzWp���fxq|O���tkfn?Y^���e�~�{dtS�uu�zev�[�z}\�pv/���sg�y�{�_�m�nuo�i�v��xxgf~tp_guPe��s�qW��f{�o}O�Yk^q��\sPptg�cr|}r~|lls�}}|E��ohk�^|=�^�qg�_���}b�t��`��v�mwqs�i�_q���|�itqg���j`yut�e��hz��xyh|~tx�D�w�GYs�r]�ktz�lk�U�j��y��s9�xvl�}|�hZg|w��unu�r���]iyjw�t�hu��wt�kg}m��lnsgnmsti�ey{chafl�gziww{|TW�czfsV�z~r�~yd|UNvf{n}q��inr�s`iyS�|�vrWu��]x~��uz�Grwuplrpdn��s�~|yjwwrn}}��y�uo~��xiw|nl~vqktw{}�|ys�rk��nz�lnrmuium�uwrd�e}�xlczs��uo~qe�ul|cr��zg}qtyhty�r�q�mxzy�zyoowx{�rco�uwyo��}v|wqjzlztqu{{{zm�|�~�v{q�gyywpk�d�}qqryu�u�{uuuj�n�kv}yv}ymq|ppsptx�vwy�zz�zomyjsprqout�b{vwq�wzx�|q~xtwwyqop}sl�k{�wr��nv{lg{y��|~r|~z|n�qylsww{|mo{{xv�km~q�zwuuusv~o|{�mzuqq�mkzqmzvrrp�n�}�oppv�l�m�lwy��xx�{r�xzrxwe�r��o|ps||k}nv��q~{{�z}��rm�pW��~o�v�wr��pw��weUbYs��oi�bw`jow��n��oz{x�umq�yg��zw�}sukxxy��wrgfd{y�rq{��zl��sP�gd}�tshwl�{lz~�heu�jttzm�pT��gkyzgv�~|K�`c��~[if~p�tn{m�|ph|jek�nW������|Ur�v��o�}~wvuTtWv�n|�zqwvw��jk{��kl�wo�yo�zRt��Z�{}�ru��m�o�\dlU�agi�\usc�luxkugpbj��k�~vi[�Xm�]�ysacx��t�}yblaw�z���wb���v�h��_}|o�u�mmuo��z{�_Emv~~�x�]`�uaZ<��ylg�r}md�nkox�^��s�Sm{�Qir�Vgdw��yx|op�}lwj�fnwv�`�rmi|��r�jsqlvm�vVzlsiuwo�s�gl�l~x�}~g{dZhx�}~�F}Pj�~�pY��q|c~��l�o�l��loibfu��njt~�st��TEv{}or�Mwijp|y��M��tjf�q�d��h4k~rk��Zb^�yr��g���oyOsel}�fjq~jl�ymauq�eZs|�|po}�tnqSv�}`k�mt}x�l�p���Y}�{xz�y\n�|v\nl�Ykj|�uyqjvk�}}wn~|�gvY��a`I�nw���van`i~cpo�yh���dh{~hrF���|v��z{vs`z��w�|v�mja�m}c��ld��R��}hoi��Ypxur{|yn��hzx{��p��n�tsL�b~dxq�g�lh|nump���l��:���uKk|qrrzzSnnmt|zyi��c�r~tmtsy�s�zrzns~m��sx��mk�vkk~��xV�olxov���ul�sn��O�^u�smn�vyr�Njyrlu{|rynt��v�ZoqopunW]�x~k�A`VZxqy��dhf�mzm��si[�u�^}xdmr�}aY��t`s�v�z�fyuQ~eh���V}wr�d{�l\t��hmbS�{���_e{{w�kpl�s�O����~�z�l�TG����vi�a�]�ZL�m[i_�px���k�_k�yig�}mtjj̉l}p�w{]e�kguu��z���k�Ttm��q����Q�k�J�ww�@v�|m�um`}�wm��^�Rst�������rb����x�Pn��we��gxT�r��{Slp��v�rx��`Cq�z�t�F��Y�o�j���aP�F��z�jq���t���yw����m^rn�d�u�tfxiq�tq�Thfu}bznjpi�tn�szhzis{�X��q�vo�o}��n����b�s�snN����jd�~kqsyz�sx�kx{p���m_kx�rf]hp�kk�i��r|p}lx|�e[t�}���stj��z��jwohsh��}mw}o�urg�b�fVb�y�vc~�f���z�u}z��zr�l���kswku�j_zr�v�lK{�~uuwue��|U�r�{ycr�q�x~{�t�vg�~pw�v[y�w���rwc�u\qrk���r{qw��c�l`w}W}qjfax�kow{o|[�tf�qbg�p�o}~x�e?l�jh�utvur�x�ko�x|{wu�w}�q�jewevPz�|yqy`v~|j~rujh}\z�zu{�~�l��z}�e�e��bo�qxnth}r�}ri�~_yfx~y`T{s�{j�dj��]z�Vm�i�{~f�}���w��k|u�\�~zcp�Y��xo|y��vq�Xku}h�{�{ql�nhkjqu�icyyn��wVhvq�q|��~k{c��|{�qkyqh�Fxn�ss�leXq�r`m���wk{w���b�h�my}ttl|��yqdfkwm��s�nd�zt�[|rwdy�}v�y���}t{r~vLtnn]qu{xl��e}ychqsjxo�}m�b�mzjt�t��unwzp�frY�}�yl{i{q�ukjgv_�{esmipy�y{snfrmpe�ym��Xs�er�_�xk�[�o��~sys�xUek��xh���gt�xo�{�|��|�i�}{Xez\irdh]�t�t~�[zzb��qn�w��y�}x����je�x}�fwv��[��wcjq�oyjZeq�u}ru�|na���e�}��i�u�i�j��utH�x�&]{�sTsk�|{buliu`^�}k�{�|y�Sk~oIm��fbR���S}~x��~z�vhv��`�r{�}m|d�{pz7{wm���lmrmq��x��g�v�D]=Z|��t�x��M�r��ouXkultSa�qh[��>Mut~p�d|���yOe�vz�ykj�g�w[yt���~{`���}_y~{q�ys��c~swyU|�mnx����pr�p��y�utt{�bs_e|�{{u��b�f�qzz�iqfzf�zru��|yln��y�zh�wra�p��{inzjMt���|qfidshk�sw�eegEvjk�kcaer�r||m�u{wpwvz�tk�znt�w�n|��Nr]|z�mx��e}�tkt{�~�}{y�ovebj�gpy�d�}o�Erz�s{�~\��xh��ff�x�y�^re��fxVzozf~pLa��}~_�r}g���m�{x^rpYy_}hfe�qh~�Zkwp��sg��|�ikny�uhsZ~z��ii�}y|lr��ww�nV�oxx�ox~}�amnxk�lx^|\}�_h�vm�ca|`havEo:�j�}[�Vyx~hwt�aa�`l��uw��s�qdv�fzl�|w�rk|t�i`�wn�s}zjnnu���s��pdivx�ij�er��dtn]oo�~�|s��o��s�z|�sr�{s�}Uifliruo~{p���rcpX��{k�wsapk�d�_qw�m����nsX�{[s�{��d�aogjxj}I�|��swoyc{ko�u{kc~i��uws��Zt��s�Yh�n��pxv|}�weo}{usx�zv�q[���lpbVu�syfk�mj��ts�k��_�g��vmj�q�_���`s��p~{s�hS`agx�v�k{�Yev�x�n��Db{@�^�{|�ql��t�i~m|s�~wm�nm�x?jjU��yv}po�vt~����zV|[pj~}lxo��|wh��uhs�rvsz}�~��uss���?y�kw�w��R�_l���}u{{g�k�~v�pp�i���xio��~�yPv�qTkldsa�x�Ed�^zz{uvqhXy�t�xyjomyy�tl�b{w�oqtp~pkWo�{l{hSk{�{v�ayquxm�u��r�^ytM��{pf�x�}z���}r�^m�i��Gzy�v�rfqpdu~u�ino{x\�p`�k�mre�l�giygl�syjns�{^a}u{Xo~pmm�xUvnhzo�e��cv�h�qyk�J�Ujmrh�fe��xc��w\{qpT~bxktd��}�\j���s����\]�m�3ypn�fqq���ehk^qlywxx���vi��y�j��coPpU�fom|�����{i�[V�Rs^��mb�Xe{�Ed��~U_zZ�i�������clg�]y�u�P�{��q�u�hDrd��`��ai�egpYw{��[��uy�]z���LvHe~�^�{���km}]q~uts}pl\j{�pww|�{xtkutq��fsg�y�zjprc]�l�h�xpmgb�`~r}pkyuhp��k�g�pirxzj}x�T�]�i�mfg�kuhibhuj��o�|{�rv�nn}�n��n�B�|���tZ�cptoo~ig�}y�o�rg]a��s���lr�vvz�|���}nxmtw�XZ�~|t���kwtg���}nuQjWd}��ch�w���wv�xf���tj|z�g�{ikmr��O�t�n��kwl�_l]wr�zIQ�bxvfsoihwi�\�cs{uvl�w�`�j�zrL�tc|v�j��F~xJnr�Z}jx_U��U��o�sv�y�bup��sk�go]���ze�_�j����^��~P�ts�rn��iyu���nqs}�~kV�~K��os�gl`B[��gw�{r�q]p6i�q~{x�qn�e�mq�u�lqd��s�s���Qu�s�dj�\w��t�~�}z�v�x�������?rK�e[b�gn��u}poiĎe�i��t�\��p�~��~z��lgo�wmrl��Yl�v�y`�b�s{�|~��~e�r�Wp�{xth{{V}P~zxXk\sv��n�zi�}uhv�ey�e����{��e�p��u�zy�q{s�}wz|��x����lib���n�Tmb�l�~m�o�up�m�~���ne����n[�y��_Sis�\��yp}yrw}~�`�z�}�\�wh�}_�fidld�e�~i{s�}�eys~�fZ]lJgv\~�vY��|ps���Ko�|vd^_r�~i�Eo{b{��b�|tj{z�jd�su��\��dsvy�Tt�ixy�ibup�n��\`���gtyf�q�e{Q}f}v~u9OUwa�ntkv�mS2���tok��?u�y`��w�fx�u�|���[f`nk�y{�eu�Z�|���}�vr�_b��w`�|Yzn�|��[ogs�zi����mbgURlk��ux�ukadie�h�i[�hcc|�`�J��o\�oqnP҉��Sl���a�i�cUm��^�knym���u}�p���tr��YeXmv`g�pu�vwps�l�y]~t�i}wz�t��ktde~���h��wvi�|>��m|t�_�xq��]�ont�jKjWuoj�W}kq��vs�n�q�uo]�|���r�ezs�m�}ta`�sr��mv��vsr|^�~ji�pq~u�h�k`z}`�nuv|oc�k}w��s�yhwGNp���sfhtnxk����yY�z���uY|pozkzvNopoj^p�xXo��q�uozj�qyui�Tbf�sc�G|mbt|}�d}jnozqu{�nan�x����psqi�|uf�i�xu�^~�q�~��[rufua�r�{���{th��w��l�{urgqt�p�psY��\dWxhb~q8b_pppqUf�{s�~x���n}_mt�pp�~�]�vrv\�oqlw�trwp�xk�W\_s�dhz��wpz�vPvj_��W�q_��l�sA�k{�prc_\��^�o`�`kkf��`�bv���g�sdp`TP�}xX�tyI�vhU�a�Xm���zxjaj�wrvPKPZ[ob�~�kqj���gr~��x���r�_�xEk�wx�}��urjv�rww�����uh�vG|w�}vt>m�jr�t�}Py�kqv�wge��xw{pdt||hkpll�ws|v��RwdMQu�s��x�Z~�����ilT�}r���t�m�Z]Yh��pfj�_iL�bzf�������yj~^n\���Nq�|vzxOU�^��G�q��xl�~dai�k�����V�jU^y*j|~�j�h���^lq�����0�^�~�^�]�etF��h�b�y|{j��k����n�l�dhPUr]ys���qbeu��`_m_�����`J~�twiXy{o�����zwslzz���vk�cr��kuj}��x�q�y|z�n�k��u�c�yn|�bi{�pfw�lh{}��|er{��pe��mmmVp�{ln���}��nl�_myg��ts�sr�{�yuk�q{~y�wme`q�se�Oqbh~u��ikfztfdnrqw��{z�w��lry~~|�pllyf�xuhyu��cshzz�zdolpul�Mv�]�u�o�pcnv��s~h�~rsihm`tT~]vyrV|�����`T�<u������{�ki{|n{�`���yrrt�d�q�y���j�o��{�nq�ey}��bvw��k�_U�vhnn��guU�iH�Vngw�vvk��kgy�������[{x�pzw�eYpqrq�ih�|x��X��R|Zq�w{m�q�rk|w�dbf�}a~tct�vpp�oc�t��|��{z`rciok�^w������jv�cq�|<�pu���qw�kYo@~q���d|�lS_�o�kz{fr|S�Qdk�x�z{wp�b�bj�uh��T�dhj�nU�����t�V�skv�q�v�n?t�q|c|��wv�x�b��Q�qgq|�lyu�^snv�srx��w�Z���i���t�c�vwt�\�r�c\�r���yumP|wnw�pqj^\�ptp�[d�|r�xtZ���i�qshoo|lr��c�_ny�w��s�_w�lSc{����[F�[\F[onvr�ei�k\a��o�nx��sTlzy���qv{�r~|ew�z�Oi_q|Ut��u�voy�u�hyws\{|s|s{psgk^mbn�s�vrwllvlnh�wem�rc�sS��l��pyh��ycq|Ff�]}����\_�jvZ�gd�rvsl��t�ey�rppcuq��ywmz~sqa�w���rs��Quyj��|{in�zTs�|�N\mn��|br�|v�L�Z_h}k�_vh�W`up\}����o���gytYkx{|��k�o�dy�vuUh�zkf�[ol[fqm��|�q{dXkupZ�y`�S����vqzkax��R�sf��xv�ko�xl�`sjy}}v^e�w�q�s��Q�}�qs��k��_t`gwm~rbS�r{���j�r��[u��|d�c^�quxy�|vm�f��j|z|�����~jbw{\Q^�n|�x�oum�����qu\�o�jnvkatcuk��oYz���cc��__�~���hmMqcnv��s�~�joytwg�nTj�����i{}�o��bGdvuy�qa{��u_{vy��ux��fulw�_��z�clnq|w�~�irz�e�{y�uhq�mee{uzz~zg��Xgj�nlpz�|vtig}ucok��n��ns�pp��m�hq`}u�s��{�m~U�y��u|dt��~jk�lm�{|�vlr{��{b����if����r�nwjyz�v�lnhl~��tw~[hp�n�h��~_y�\�dq�z\�o|�bo�vz�sn����z~]q�e]�ri_hh�mju}csuafrphTkxx�r��K����zj�emyv��|~tX_�y�Wyk~VahCej�|���KW΂h\�o��zl���j�p|qm�jwns��w�_��H�[��cXvfv�Oc�V�j�snsbp�nm�om�r�x]nr��swud�p�lublo�md��zw��n�t����}d�p~�k�ix��h|Y�g�tbnmxy��cv�zl�nvm�Qr�vsmkym~ls�xf�ugo�|e~_�j�k�tyW]~cap�m��~~kuuvq�Y���|z�t}�s�|cr�ytzr_t}�epi}iuy����vr�}�|tP|fq_�wglsg�t�~yy�n�v��ydx|�y�mi�x|��juw��x}yoxf}ssqsst~��x}�g��{�~{��|�g�m{rmr��wbvs_Vs�Wzy[row�z�q�g�yh{�wv}mz��yngY�4�rv�qZ�]�yf}p��xwhL��eQ�sx�����kk~�|�kasX�lquntxpf^�j~�MaoZw}�f��ywjpyOboqvzw�s��wegi�t�qlar�~v�x~�tNm`ujw}p~iu�s�ou��`�v||�sQ`��wsb]�y�fwsr�xbs[��^�v�f[���xu�m�m|zb_�u�mz~~�t�i�N��an�z|�s�Y|uGvwpl�ty�uq|x�~g~s}~�|~�w|m�ov�zh{ksohry~ym~|bh����af���fx�jtny�jy�pk��p�p�zl{w~ix�ptv�qqknwyU�pwt�vY�trmpr�m��`s~~c�{qq{_hy��ojml|}y}Sloq��s�g`}}hshy�Z����v�mrnpaqinzJ�c�|pk�{e|����i��|wZ��z_p�xl|_nbf|`mj���\ds��sZ�_f�h�l��ve�w\�^kli{jnws�pzTe�|u��j��B{n�pl��gZk~Qer���\c�=t�f�sd\�{�xS�lzn�fk��tn}��kp��qv�bU��q}��Z~m{p��iPd�w~�tvXPd�{r�iupr�xwf{dtr|n��zur�sx�~vxp�u�pzw~�ks}bj�kif�o�yxt�pj�y�uw}��zi�oo����u�zqz��|yirzp~R�t�lvqiW~z�m��h}�u�b�om��q{�yF�h�{�Q|}o_w�h�d|iwigvq�����ft_y�nx�}�sm�kg�w�nd�c��soQ�r�{l|�hh�xzYyuhq�zv�izs���`v�sn{}d{r~W~d{n�~�lu�my�ic��8kr�qz~�qs�pxK^s^�}mpmx�w{�qluq���l�pd���ubyrijmexd�v��ok�}wrt�d���e��Idxih}��z�qugj~hw{r��r�mub{wkj|s{�lx|bylyzkUzu�cku�}�ty�`pY�Yu~t~y@c|ou}lahi���v[�nq�f�p�t��xVjeUi��v�gw~��Y�V�msTp�GT��o�rb�Vyr�{�h��p�zJ}R��f�yXwkH�a``�eyu�bew�b���E�|sd�nI{|~�yk�b�f_�m�_v�fm�t��{oYol���Zf��oi�{vuHzwd_ih��fio�`�kl�vo�Yln�zwPGOJ]a���^����hh�p�mu�^����mo�ssNzSd�i�n}�]��Y��r�R���|�fW�X^d�T�D�|~Xit�\Yml�{q�Z�w�~fxSyny^T}�jrh��m�n�s�vi��zr��hj|xskh�v���Ut��hvmZ�t����p���tU�mnxy��\��}v�uI�wlx_�yd��xY�x�W�vovzn�p�m�l��xO��p�Nq]|`ne���pxbrUfxiSP{gluf{���hlw�sypnn��Uv�{{�}upz}szrv���y���~r�k|g�{xiu}tuwmou���lv�||st��q�d�mfuxp|vgz��srwk��zx}zxsq|us}|y�vy�~}u�zy�oqs�suti}xomzy_�����{quyw�k~nyi�pvxxlpt~ps��suyyymmpxs~hmzqtvu{pvx{otsix�i��d�u��qdgksw�~va��Uqoj~vnzth`enqshf�vl|~�l�x�o{�y��k��tn�pzv��t`wnh�|ujt��q|{ti�{ov�t�mj�{�k��s�s~v~ys�Tx�o}|uk�iw�vyhk^unvvw�N�lwsb�_r~��`|va�{v~kqu{zfZ�emy�k��s{xz~_at�ct��~}��f�jja�hq�s�Su|�c��wu�movgn�[��^{m|�so|�bu��r�hq�j�q��kvwyaxTtum|_sQxuz�~~hsc��lsouu^��roFg{�ur{�my���i�{qo�|�x��Yv`�qNn�|cc�Zi^txr�sk�o�ir�g~rwkx|�vomk�~���t�xX���M���{j{ov�amf�s��z\}���Mu��v��rpZ�{��uv��f|sm��yld�on~�o���}r[jsixa~�j{i�y{~p��c^h��p�uz�vjww��f��nu�u�~�J�u~v{|�_W�jryl������qxsv~uhwy|iM�����ei`|�eo��|Rm�[oxndolgqc{{yV�zglitrlxC���Y�P��lnm�\�s�{cnnyuyx�r�neh~~�{l�p��qlq�c�zu~��_lt~��viuzsaq�pt�zf�hqZ��c�wdy]s��rt�yflk�tyurff|uwsyxYpxj��q�usdyr�x�|c~j~��is����hpd��{ay�@edf�v��{�y~ht����xyw}�|k�x���p�W�tm[vzozn�}Wcge�]z�a�r�zi��bl~��rg�iy���w_wyu{�wq{{�adpjxicu��V���|�`{�usyv�\�XxH�w�h�n}\tf�q�r{e��|�u�tvfvxgsxuxsh�y`xn�yka�xs}o�wruo{vus��bi_����d|r��svtqbL}aHgzsv��t_g�hnx�yygd��n}�rn\eszzi`l��w[unq�w{�`qny^hqnu�u~ca�Rxl�xw�uu��m�lvl�s�|}l�kwl�nr��y�w`rg`u{yui�}�qY~p��rc`m�u�e��uci�nj�~�evmu�{���n�wijt�ln^�sp~t�|quu�ze|szw�lc~}s�jz|l}ripgx�j�y�m���V^cxkl���ylWf{�e�^�w�v�wXbbh�iiew}uhnx�����fqh|Urzpf��vW�`qvxp��h`z�k�n��wwvWd}{���R�n�I�d��f3>�Q�an�tc�~|-�N�gsZ������\�oL�N;�oa�7w�)Z���lhoeP��VW|ĊF-՞r��]}��|}p��Tt���smK,X�t|my�~�]{�F�}H�f^��dHgb�R��G�Mop|wx�jbDjx�c2��O'tae�k��H�ok��G{�A�t�s�_YS\�x|gs��^Y�}�v~��^���lp|�s�y��qqm[|{uk�z��{r�tm}�tobvz���Y�^�cn�W��xz�~v�zi}|�a��uq}�}u\Xb|szovxlq�n\z�~��mtix������}ewb�y�`�xlU��|���pswy�xg{�jjR�[�ctk��ay|�z|vj�nji{}t}nt��eC�i�vz�zyi���{�ww~|feqabmxpZ�lc�pH|wx~�}�k�x��}��kb�k]�]GOy���x��|�`}{hE�{n�������rtv�w�xlc��xselT}xx��jv�rk]zq�q�k�tuS��T�~��_��i�r�hc�y�oPhtV~�u�����w����py]qc�khytm�n|��sypvn���|b�v�Z��mmu�of�tL�uiz{�z�|rny`�c`z�ir�hi���r�lewe�o�p^{k�f�kt��e�SuW�kzRo�a\�\zo�o��wf�|zthm��J�s�����}{�c����^fwTcvUoToo�lwj���x�����lo�xwq���kl��Zq�h���Jv�TpW�|~Es[�ga}�u�\����cw���ykkF�l�rus�fm�v|�`Wk�jLe�|tnvk\��F}��rvl|i^��w�g|�}ukorush�s�twsnrhtum}�n�~�uzc�u{v`tx|�e�joxhqy�pxxvwq�c��y~nuu���suzsrnohpxzu�|pzpxpx|wpz`{�r{{lVy|�okwrqx�cz�nwiip}�}s~m��}l�y�}��w��uwv{vu`km|s�}lg�|{��t�y{z��cp|u�zx{z�|q{uyzqhxray�r�r~�zwswaxw��lh~�vmozhmt�q�}oyt�o�t�~tqn}nou�{�g�qvj�u|��rrpog`w�{���{��r���o{tunj��xw��fQ~|p{��ep���{z���z�jp��ks�n�w�vw}e��mzm�g��yr��_�}�nutz{�t�e�z�g��|��o��tqnt�tev�p}{�wmy�sy��qtz}�fotG�o}`��{u�zv~�jljM}p�bt{��b�i�x�d�lgv��}jvq8i�[|_|sd�z�u�e�k_prn`�szz�za\gg���pi~vjY�Rd|trx�lW�]�bcftqqj{_p�RdolnqfjdZry[ojs�kr���i~n~�~�Q|{ewtQ\a�duwo{l�p��}rnv�mW{l�eNe�kt�otf����n�tpK��V����|��uzn|nsj}fX{�}xhzz{��~cvq{��d�ww�^y{ubp�}��Z~��q{vp^g�}ysz�u�lj�p�a_�`�uj[zm�Wwi��Eq�w[p��fvq��mup{s�s��{�~�g�wrn`�kvm�a�zir[xxm���cy}�~�mtr{r�xd�[~��Nz~��Wx�����u��ux^|izozw`�qn��k��kL\�ei}�cosudmxn~bxvxT{�|sv|ytnFw[�y�s�F�Wxr�p{{i�x�x�j�{mv{qtJ{c6pygswyc���~p�y��ttcv�t��`ry�pz�~����z`tmkts��dwp��gr�v9����`o����MykWal��Zx�wqbvlTg�~~r�x�PN�[M�{�z�u��]P�wX��kk���u�s�jq�z|Tc�wm�pkkscz�xx�{k�o�xi�v�m�r�`x�n�kj}q�\}zp�iar�Oh|jy�vqw��c]��b|jHl�d~S{�}��im�y�puw�ysn{t|joi]w}�cgza{�Rfx~uyst�y�~�ksq\�p�owe����l^�o�tq�vu�jo��ux~t�qrss���ry�_h�y�onu���s�rz��g{jke�s_sf����ix\zp���{jXcw��We�~gzelySip{qy�d_n�\y�|{g�w�prk~[|�fg�xouwjW�o����ps~k�|v�����Zz�e_^o���Z��ol~n�sum�yjky�cmUsoi�zlcUityn�Zc�}{��lqz{�|w_R�~x���z�oaLil}|ilq��ofyTkrj�h}u�p�kux�vf�l|��W�}tNjcxh�|^�nzz{Vt�z�P\ie�h[ql`Z�oz~x�|v{Et�rr��s�h[�liwbm^�Z�|�e{ppo��i~�hfnfskQ��_�~zVz}q�d�����mmy�|UsQ�ex�g�|�g~���l�T������z�t�pyt��xRXt�l�o��u�xWn~�x�gy�e�roh�g�}���M�l���htu�aj��}���lq�t�{�}����Grnwp�j{k{vqa`~wlk]i�h�\�q�s��bkt�{pj~l{��v���{r�y�gjgd}���iy}wvvs�zk�l�q�sT�r�ovj�kopi�~���l�ewl�y�|\��h~n�z{�Xyt{zq�[wc��}x~ugZxt�d�wyz~sz��sq���T~vv�jw���pqqr}qp_bUu�sx�{iqmx�uo�}unno~yr{et{�w�|sow�ksn�~�trer�oq~lsd�~ssnq�uu~di�n{mx{lwp~�zo�xn}i{}�yluth��jt^tl�q_uw�rs�roqsq}x_�z�ub�r{uqlrw�~�{���ihglm{srtvr|oyuc}k���lz���fu�pr��k�nkr|uu�`~s{z|j�|�{ynvewx�|urunk|�tkrn}hytrvxtuo��wn���~urxu{u�ttzxq|\�r�|�vq{u�~{�v||��v�v{|qgs�|ux�|dt|r��z}u~u�zv�mv[s|jzxym��~t{xy}�sjuvt�{w�rp�u~t�|�z{vxzq|t|gjt�s�}ok�y��sp�w���vkw{yrst}rvtur}r{p�g~tj�nwb�{zspxviqxqaoltw|n��ve�y��j�|{{|mwykzrni}jyy�z|hp�zf{xxr~�m{soko�yunu{qpc��qhyr�n�{wn�u{�}|zy}}R�roi~uw}ul�pq{qn�uz{zsu|�n�x�worqs|}zc�f}y�muwmrysq�ys�ix~vvwz�o�vwvqy|yvmq�||ol���v~p`�|^uymT��qzmwk_z{qia���bncr�f�lbo�|f�u�mzTastpzsebn~wk�}u`g|b��n�`lws~��uc�nm|�{�����mrq�t�px��po�d�ult�uo�rw��}mt�o��~dt�l��y�|�Q��yex����u��xN��Ev{|Wry��t{�h�k��q���gX�{Ygku`�u�kuo{{r��y�olmpw�SZv�yp�u����p��a��fwu���qu��sX\�U�kX��tm|]\o��p~��{_o�_uWwx��My�}g�r}����Zr�����{o{h��|z��qwyh���cgl|�c{��x~pou|cugu�`s}k�~mg�f[~���hYyh`e�nq|<vhoa�_�w{vuxtroc�d|�w�g~b_n�xq��h���Vrm�~hq�m���z�vV�\y�hxu�_�l��Jgm��\t����m�|\xk{gw`uy�tz|h�s����[m~k�mam���oy�|wxOgv�tf~�Ww�to|asa�Y�e�gy�xspn����xZk�ty��|��{x�z�||jpbxx���ur�yk�mv�v�u�vi}k�p_ks��m�tk��vg��h�j�un�u�]_zWx{m�_l��`�xp��rn�`�d��~���wp�dyjao~��x�k����zwjjxnk��Mr}qax�]gR���n`l�{���wNjbz`]kohp��fk�lbofws}v����o��Xxzm�u]vu[[��q��k�>k���qu��^tV�so�u�v]Qx��}��sXvv�w�`jw�g��evk�{�|Zlr�|sel����zn�S~�����u�yW�unw}�~uoyuxj��{�{�\o�Qg}��k�_}u�{p�_uYrv�v�����y_pq{�nym�h�ck�^~}�yk��}b�x_��vs`�ixds|lN�egk|sjjiZ�so��d}�~p��q�z��On<�^�}�_�pyv�f}p{fw�qs}t�yfy��oTn��R�ndk}m��{{{�lj|plvpnhr|�a�lg���yysrg|�nx���k�{nO�n��i�f�l~gxWt|}y�wdtc^��}�ciq{xn|wf|jx��ouj�}q�wKe���umr�~|�hw�lpz��yioh���|zxy~[�ndf�|w�g�nn�_��|}��zfx�xm�|b�s||y��~�����bslt�jnzczh��h>�Y�jyt�������rs�qgsnkO�ft|y��pi�c~{o[j��z_rw}�uvnov��uihiftjYpN�N�x�msZu�|m�]tm�waw_JM}|�c���i�uh�r�p�sy���y}��X�]{h\�woy�k�b�mM����|���e�|�}u�gk~�Y�d�^Pnvlv�@�|yfa��qb�wI|VFos\|{�wx~c��_�l�o{pjW�t�h{{�VX�is��s�hyG�h{x}�gi���n��~�lg{mq}mRwvs�dc��pv�~���Id{�ied��|p�u}]t^VE~�fH�Mv:�q~U`l�w��{l�_��kr��x�t��Rzsz�s[�yt|��{l~�|ptdg`�wilh}m\|S~qg���\�{i�n�n|xv�o�v�{{p\��x`qv�k{w����m�vm��u�vh��nZ�us�i~�afn}u_�ϣ�|q|rY�vgi�uBrpa����hqp�ynfv����}�^�z��_�d~��y�`D}�z�hUWx��X����Gt[�xn��|V�i\W�3y�oh��Z�Kaf|o��C�[|��{uNh�pijms�bVwFi�l�R��[����ypa~�Z^���mrJu���bV�lYK`hk�[���k�zKt|l�.��z}�Eb}f�q��`q���}yc\q�oapvc|�pdZv{�V�kq�~~b�{�u�vivx����mvw�d��v�h�h���govs�k�s��_�T{�h��~Da��r�\���wO��x��\��rN�b��h]fFh�h�y���azm$Zfb�p�uO�cyral}vx}~��Y��[z{_p�w�g�NZO|y}]Zrf�_H�Ylk�hrzAv\�RWj|rx�{yoFWz�fo�kp��xrv�w�c���on�x�US^iVq�`t_R�y�yh�Sqy`{���dos]fYS�ewghizO���j{uggi�vc^�z`ifq|v�Su��u�qsp�\jvX�����n�zc`_>ow�wlbrmt}����zaogbw��i|n��p�jwqj�jzh��pQ�R�wm�n�htcY�Xqa�v�lu[}��{quq�j`y��upShn�{wq��ueSze_g���ufy~��s�Wz{��r�TaZi�s}�d�^�v|r�xX^lN��j}m�Zq�ptp��ov��ko]n�r^z�k�p�jo�b[�n�l�j�dfxs�wVP�yvrd��s`s���{u|nko����xqaie|tvlv|�uf�g��w}�nXxk]~dh��}g��gzf�x��e]j�~t�w�}�kfl����kj�vRm{yn�v}���u|r}q�f~o`N���\��iu�zj�s��}hjip~`sw_lqf~z�{y�\xhv�mlxkkyzy��v��wvu^�{�je�vi�f�]��|�[�{|�rp��_spr��kc�\�xw^��hzrs}`�smk�{yx{uw�{xvugtth��F�x`�ue���p{iorxrfr�sp�dxv|y\r�o���z}��ao]i����y��Gx]��~uzx�l`�}U\�xv��g��\a�ai|��tce|wfa|o��vgi��qx�w��j�~�|z�uq��wopde{�vczzOv\Yx�v{~���~rlUvog�u�|i�m��{ssx�~�c}��x�vjwvzpgqx[|��py{�bui��~ztg��zzjrmuq�hsyhu�|�~}�t{�~�A�|s�b�qfd��Vx�py\}�u�[{���o����^�u���yyo}�l�iowxg�q�uyuxj�u�Zp��mm��ps�m��]�hp}r{��kvhiTn����[fw�_yd]��b`�iO�jf�Yz{`v��o�dp�q�^ar��vy�p}�Y��p��i��wx�h�wu���{e|��}Yq�hwt~q���z�fvy���ll`}|dp{``h�isljky_s��Yhy��`�f~z�eh�rgx�}nc�c�t�ywfhb���old}�cs�p�uw�z�}���yu{{a�ohz�_�mWf�i���xpjtt�t��n��|yl�n}eY�Rcssp�wzkq��g�}��gxb�u�p~t�t`~wmy�b~�Rkyk]�sN�w�z|�p�qtn]i�~q���m�gU}�iu�ttw��ai�j�h�fp���gzia`��L�hjC�z||�~v�^dss��y���{t�t����pkw~�lw�s��s��inl{z����au\r��u�ume_��H�cnlow�~p\^z�ftOs�����qm��x��gPnfaUyuzu�iD�k��{zl�^�nzg}{~\�crv�}feD��w�u��YLor�~oc{��]nj|uz�sxg�bm�svrs�p�yhp��~ut{y��bvpy{Xg�btqzxo��~g�i��h��tks{n�ozm�s�hxm{�||{k_s^a�x�u��q�_��i�v���em�sl^��o{plx�hneuj{po�p{q����yhdq�`�}o�zci��ctp�pmlz�nw��sTsu�u�s|xfg���_q�ks�nmk~|j}hxw�uf{ev�q}�qpkZt��[}}�_`��`�qbfip��u��abqhb{E\.���gpvyi�j�b����B�o�dOcsowq�uYcqy��c���n�W�R{~frX��x�F��jox�xw�QgXkbJh|s]lrf�|���Tvo�����U����^v�fes\dD�U}c�p�lig��q�Xs�|�s�f�P��w�_fl���W_���o�v|�t�pBC`�oyrN�if~qy{�kq�j�xl�{�����TSioqZ�Wpw���x��mos�euw^_��Ymy��_m�~x�r���u����Y]��pa��WVwyzljVn���j|}w����k���vwgk���c��rw�}djj��rxo�j��h�r�rfnctzg����ppl�ecfnl�|���iyy���mc�xu�Tb�gv�j�]t�T{gx�vqkrOni``���f|j_���sue�P���qp�ffuI���wp|njZ�dzk~cm~h�ryxm�gs���sKoey�l�y�t]�~nhisz}w��kvs�tt$����k{�qe}~u�hv~y�nsxlspw�`t{n�sT�z�ojVOo�x�lh^t}vqM����mey��u\�sp��z�c��Rl��vp�wq���feyco^�xjMm��}k�zw�a�vya{]�wv�_�zvuba�qmqm�msu�����l�amb�~rp[�mkMw~{�|{�eUkS}N�]�{��|NwV_�ys{�sx}Bbx�����csu���p�twyxx\�g��mA�wugr�kZ�_^xsr�~�dx~�jyzbtro~�dqsiodq���lWnn_�q�a�ne�[cvcj?�y~�|g��tsq����m�%���s��b���C�~v��m�c�tp`~]1w�g\Vwku�XfE|�kF�jn�;arEx�bf�_|��zx��pkx�f��{uqQYq|�y^d}��W�Q\[���wZ��w�_��}V�f~�x^)Oa?yl[T�m��W����Fc�cJbZ^�]��dJ�n�v]�n���JWii�j~p�][_|��g]T�n����J��ws|E�dz[u�um[lwhm�xsptXt}gnu�{��{pzz�z��y��gr�|t��wg���Xooqgi��k�t�z�xxU�f|�a��`;o_{�bnmfxoe��d�d���|p��|o�s~mwm�z[f�N\r�^eu|vylrx��u�xmS_hx{r���~t�x_�ry�pnhn�sy�qt\�xuo}��Wnj�t��i��|os{os�|v���y~ccj��pmplizv~�xir~wb�owqiydse���qm|�gkxu~�q�bnrv|]|si~otwxyt{�[�qXrY����evk�k��t�ce{�vty�in}�zkx�_}�qxo�n`��prriu|�y}�zTfhyuX]����y��ri�o�zd�wsp|hxn|k�t|ut�n�l��^n�}�krp�ui�uwq�lt���z�}ll{zu�zp�}�{�hx�xq�~|slwt}wx��v���u�s�xdyvk��z|}zmr�s���x{{��|v�{i�ovp}p�z~�zvv{phsnz�v�}wv�m���{|\ku�ujtqlxq�o�|p���v�zjw{�twtq�s}x��lyn�yy��}��}~|~i}|�h}kt�qxy�r�vk�ky~w�|m|tvwupw���nxz{}ixm�kn�q��q��}�p�yq�}}�rs���d{nkvv�ywjqypcb{_v~zo�