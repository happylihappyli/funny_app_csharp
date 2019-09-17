

s_ui.SplitContainer_Init("split",0,0,500,500,"v");
s_ui.SplitContainer_Distance("split",100);

s_ui.Button_Init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");

s_ui.ListBox_Init("list_friend",10,60,200,180);
s_ui.ListBox_Init_Event("list_friend","friend_change");


s_ui.Button_Init("b_clear","清空",250,30,450,30,"clear_click","");

s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.Combox_Init("combox_head","/root/test.js",250,350,100,30);
s_ui.Combox_Add("combox_head","/root/test.js");

s_ui.Text_Init("txt_send","ls",380,350,320,30);


s_ui.Button_Init("b1_send","发送",600,400,100,30,"send_msg_click","");



s_ui.Text_Init("txt_user_name","000",10,450,100,30);
s_ui.Button_Init("btn_connect","连服务器",120,450,90,30,"connect_click","");
s_ui.Text_Init("txt_session","000",10,500,200,30);



s_ui.TextBox_Init("txt_info","",10,250,200,80);


s_ui.SplitContainer_Add("split",0,"list_friend","fill");
s_ui.SplitContainer_Add("split",0,"btn_friend","top");
s_ui.SplitContainer_Add("split",0,"txt_info","bottom");
s_ui.SplitContainer_Add("split",0,"txt_user_name","bottom");
s_ui.SplitContainer_Add("split",0,"btn_connect","bottom");
s_ui.SplitContainer_Add("split",0,"txt_session","bottom");
s_ui.SplitContainer_Add("split",0,"combox_head","bottom");


s_ui.SplitContainer_Add("split",1,"web","fill");
s_ui.SplitContainer_Add("split",1,"b_clear","top");



s_ui.Panel_Init("panel_top",0,0,500,25,"none");
s_ui.SplitContainer_Add("split",1,"panel_top","bottom");
s_ui.Panel_Add("panel_top","txt_send","fill");

s_ui.Panel_Add("panel_top","b1_send","right");


s_ui.Panel_Init("panel2",0,0,500,25,"none");
s_ui.SplitContainer_Add("split",1,"panel2","bottom");


s_ui.Panel_Init("panel3",0,0,500,25,"none");
s_ui.SplitContainer_Add("split",1,"panel3","bottom");


s_ui.Panel_Init("panel4",0,0,500,25,"none");
s_ui.SplitContainer_Add("split",1,"panel4","bottom");

s_ui.Button_Init("btn_ls","目录文件",10,30,100,30,"show_file","ls -al");
s_ui.Panel_Add("panel2","btn_ls","left");
s_ui.Button_Init("btn_ps","查看进程",10,30,100,30,"show_ps","ps aux");
s_ui.Panel_Add("panel2","btn_ps","left");
s_ui.Button_Init("btn_ps_js","JS进程",10,30,100,30,"show_ps","ps aux|grep .js");
s_ui.Panel_Add("panel2","btn_ps_js","left");
s_ui.Button_Init("btn_ps_java","Java进程",10,30,100,30,"show_ps","ps aux|grep java");
s_ui.Panel_Add("panel2","btn_ps_java","left");


s_ui.Button_Init("btn_cat_user","查看用户",10,30,100,30,"show_user","cat /etc/passwd");
s_ui.Panel_Add("panel3","btn_cat_user","left");
s_ui.Button_Init("btn_add_user","添加用户",10,30,100,30,"new_user","");
s_ui.Panel_Add("panel3","btn_add_user","left");
s_ui.Button_Init("btn_cat_group","查看用户组",10,30,100,30,"show_group","cat /etc/group");
s_ui.Panel_Add("panel3","btn_cat_group","left");
s_ui.Button_Init("btn_add_group","添加用户组",10,30,100,30,"new_group","");
s_ui.Panel_Add("panel3","btn_add_group","left");
s_ui.Button_Init("btn_add_user_2_group","添加用户到组",10,30,100,30,"add_user_2_group","");
s_ui.Panel_Add("panel3","btn_add_user_2_group","left");



s_ui.Button_Init("btn_restart_ssh","重启ssh",10,30,100,30,"restart_ssh","");
s_ui.Panel_Add("panel4","btn_restart_ssh","left");



s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");
s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Setting","设置(&S)","set_click","");



//其他属性
s_ui.Acception_Button("b1_send");
s_ui.Show_Form(800,600);
s_ui.Form_Title("Linux");

//s_ui.ShowInTask(0);
connect_click("");
check_connected("");