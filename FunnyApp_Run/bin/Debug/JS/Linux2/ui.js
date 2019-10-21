

s_ui.splitcontainer_init("split",0,0,500,500,"v");
s_ui.splitcontainer_distance("split",100);

s_ui.button_init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");

s_ui.listbox_init("list_friend",10,60,200,180);
s_ui.listbox_init_event("list_friend","friend_change");


s_ui.button_init("b_clear","清空",250,30,450,30,"clear_click","");

s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.text_init("txt_send","ls",380,350,320,30);


s_ui.button_init("b1_send","发送",600,400,100,30,"send_msg_click","");

//状态栏 开始
s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");
//状态栏 结束


s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.button_init("btn_connect","连服务器",120,450,90,30,"connect_click","");
s_ui.text_init("txt_session","000",10,500,200,30);



s_ui.textbox_init("txt_info","",10,250,200,80);


s_ui.splitcontainer_add("split",0,"list_friend","fill");
s_ui.splitcontainer_add("split",0,"btn_friend","top");
s_ui.splitcontainer_add("split",0,"txt_info","bottom");
s_ui.splitcontainer_add("split",0,"txt_user_name","bottom");
s_ui.splitcontainer_add("split",0,"btn_connect","bottom");


s_ui.splitcontainer_add("split",1,"web","fill");
s_ui.splitcontainer_add("split",1,"b_clear","top");



s_ui.panel_init("panel_top",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel_top","bottom");
s_ui.panel_add("panel_top","txt_send","fill");

s_ui.panel_add("panel_top","b1_send","right");


s_ui.panel_init("panel2",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel2","bottom");


s_ui.panel_init("panel3",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel3","bottom");


s_ui.panel_init("panel4",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel4","bottom");

s_ui.panel_init("panel5",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel5","bottom");

s_ui.button_init("btn_ls","目录文件",10,30,100,30,"show_file","ls -al");
s_ui.panel_add("panel2","btn_ls","left");
s_ui.button_init("btn_ps","查看进程",10,30,100,30,"show_ps","ps aux");
s_ui.panel_add("panel2","btn_ps","left");
s_ui.button_init("btn_ps_js","JS进程",10,30,100,30,"show_ps","ps aux|grep .js");
s_ui.panel_add("panel2","btn_ps_js","left");
s_ui.button_init("btn_ps_java","Java进程",10,30,100,30,"show_ps","ps aux|grep java");
s_ui.panel_add("panel2","btn_ps_java","left");


s_ui.button_init("btn_cat_user","查看用户",10,30,100,30,"show_user","cat /etc/passwd");
s_ui.panel_add("panel3","btn_cat_user","left");
s_ui.button_init("btn_add_user","添加用户",10,30,100,30,"new_user","");
s_ui.panel_add("panel3","btn_add_user","left");
s_ui.button_init("btn_cat_group","查看用户组",10,30,100,30,"show_group","cat /etc/group");
s_ui.panel_add("panel3","btn_cat_group","left");
s_ui.button_init("btn_add_group","添加用户组",10,30,100,30,"new_group","");
s_ui.panel_add("panel3","btn_add_group","left");
s_ui.button_init("btn_add_user_2_group","添加用户到组",10,30,100,30,"add_user_2_group","");
s_ui.panel_add("panel3","btn_add_user_2_group","left");



s_ui.button_init("btn_restart_ssh","重启ssh",10,30,100,30,"restart_ssh","");
s_ui.panel_add("panel4","btn_restart_ssh","left");

s_ui.button_init("btn_file_sql","File_SQL",10,30,100,30,"file_sql_input","");
s_ui.panel_add("panel4","btn_file_sql","left");

s_ui.button_init("btn_process_kill","删除进程",10,30,100,30,"process_kill","");
s_ui.panel_add("panel4","btn_process_kill","left");


s_ui.button_init("btn_edit_start","编辑登录提示",10,30,100,30,"cmd_sub","edit /etc/motd");
s_ui.panel_add("panel5","btn_edit_start","left");


s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");
s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Setting","设置(&S)","set_click","");


//其他属性
s_ui.button_default("b1_send");
s_ui.Show_Form(800,600);
s_ui.Form_Title("Linux");

on_load("");
connect_click("");
check_connected("");