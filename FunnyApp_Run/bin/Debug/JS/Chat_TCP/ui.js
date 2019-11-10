

s_ui.splitcontainer_init("split",0,0,500,500,"v");
s_ui.splitcontainer_distance("split",100);

s_ui.button_init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");

s_ui.listbox_init("list_friend",10,60,200,180);
s_ui.listbox_init_event("list_friend","friend_change");


s_ui.button_init("b_clear","清空",250,30,450,30,"clear_click","");

s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.text_init("txt_send","hi",380,350,320,30);


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


s_ui.button_init("btn_post_test","POST",120,450,90,30,"post_click","post_test.js");


s_ui.text_init("txt_session","000",10,500,200,30);



s_ui.textbox_init("txt_info","",10,250,200,80);


s_ui.splitcontainer_add("split",0,"list_friend","fill");
s_ui.splitcontainer_add("split",0,"btn_friend","top");
s_ui.splitcontainer_add("split",0,"txt_info","bottom");
s_ui.splitcontainer_add("split",0,"txt_user_name","bottom");
s_ui.splitcontainer_add("split",0,"btn_connect","bottom");
s_ui.splitcontainer_add("split",0,"btn_post_test","bottom");


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


s_ui.button_init("btn_hi","你好",10,30,200,30,"cmd_sub","你好");
s_ui.panel_add("panel2","btn_hi","left");
s_ui.button_init("btn_weather","在干嘛",10,30,100,30,"cmd_sub","在干嘛");
s_ui.panel_add("panel2","btn_weather","left");
s_ui.button_init("btn_weather2","上海的天气",10,30,100,30,"cmd_sub","上海的天气");
s_ui.panel_add("panel2","btn_weather2","left");
s_ui.button_init("btn_shanghai_qh","上海的区号",10,30,100,30,"cmd_sub","上海的区号");
s_ui.panel_add("panel2","btn_shanghai_qh","left");


s_ui.button_init("btn_shimian","治疗失眠多梦",10,30,200,30,"cmd_sub","什么可以治疗失眠多梦");
s_ui.panel_add("panel3","btn_shimian","left");
s_ui.button_init("btn_quhao_010","区号是010",10,30,200,30,"cmd_sub","什么城市的区号是010");
s_ui.panel_add("panel3","btn_quhao_010","left");
s_ui.button_init("btn_guohua","国花是玫瑰",10,30,200,30,"cmd_sub","什么国家的国花是玫瑰");
s_ui.panel_add("panel3","btn_guohua","left");




s_ui.menu_init("Menu1");//,0,0,800,25);
s_ui.menu_add("Menu1","File","&File");
s_ui.menu_item_add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.menu_item_add("Menu1","File","Chat2","加密聊天","chat2","");
s_ui.menu_add("Menu1","Tools","&Tools");
s_ui.menu_item_add("Menu1","Tools","Setting","设置(&S)","set_click","");


//其他属性
s_ui.button_default("b1_send");
s_ui.show_form(800,600);
s_ui.Form_Title("聊天 TCP");

on_load("");
connect_click("");
check_connected("");