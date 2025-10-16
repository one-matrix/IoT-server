-- Initial System Parameters Data
-- This file contains the initial data for sys_params table
-- Equivalent to the data in manager-api/src/main/resources/db/changelog/202505182234.sql and 202505142037.sql

-- Delete existing system parameters data
DELETE FROM sys_params WHERE id IN (108, 109, 110, 111, 112, 113, 114, 115, 610, 611, 612, 613, 500, 501, 402);

-- Insert system parameters
INSERT INTO sys_params (id, param_code, param_value, value_type, param_type, remark, creator, create_date, updater, update_date) VALUES
(108, 'server.name', 'xiaozhi-esp32-server', 'string', 1, '系统名称', NULL, NULL, NULL, NULL),
(109, 'server.beian_icp_num', 'null', 'string', 1, 'icp备案号，填写null则不设置', NULL, NULL, NULL, NULL),
(110, 'server.beian_ga_num', 'null', 'string', 1, '公安备案号，填写null则不设置', NULL, NULL, NULL, NULL),
(111, 'server.enable_mobile_register', 'false', 'boolean', 1, '是否开启手机注册', NULL, NULL, NULL, NULL),
(112, 'server.sms_max_send_count', '10', 'number', 1, '单号码单日最大短信发送条数', NULL, NULL, NULL, NULL),
(610, 'aliyun.sms.access_key_id', '', 'string', 1, '阿里云平台access_key', NULL, NULL, NULL, NULL),
(611, 'aliyun.sms.access_key_secret', '', 'string', 1, '阿里云平台access_key_secret', NULL, NULL, NULL, NULL),
(612, 'aliyun.sms.sign_name', '', 'string', 1, '阿里云短信签名', NULL, NULL, NULL, NULL),
(613, 'aliyun.sms.sms_code_template_code', '', 'string', 1, '阿里云短信模板', NULL, NULL, NULL, NULL),
(500, 'end_prompt.enable', 'true', 'boolean', 1, '是否开启结束语', NULL, NULL, NULL, NULL),
(501, 'end_prompt.prompt', '请你以"时间过得真快"未来头，用富有感情、依依不舍的话来结束这场对话吧！', 'string', 1, '结束提示词', NULL, NULL, NULL, NULL),
(402, 'plugins.get_weather.api_host', 'mj7p3y7naa.re.qweatherapi.com', 'string', 1, '开发者apihost', NULL, NULL, NULL, NULL);

-- Update existing parameter remark
UPDATE sys_params SET remark = '是否允许管理员以外的人注册' WHERE param_code = 'server.allow_user_register';