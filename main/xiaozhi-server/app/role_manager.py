import yaml
import os
from typing import Dict, Any, Optional
from config.logger import setup_logging

logger = setup_logging()
TAG = "RoleManager"

class RoleManager:
    """角色管理器，用于管理不同的AI角色配置"""
    
    def __init__(self, config_dir: str = "app/roles"):
        # Use absolute path to ensure correct directory resolution
        self.config_dir = os.path.abspath(config_dir)
        self.roles = {}
        self._load_roles()
    
    def _load_roles(self):
        """加载所有角色配置文件"""
        try:
            if not os.path.exists(self.config_dir):
                os.makedirs(self.config_dir)
                logger.bind(tag=TAG).info(f"创建角色配置目录: {self.config_dir}")
                return
                
            logger.bind(tag=TAG).info(f"正在从目录加载角色配置: {self.config_dir}")
            for filename in os.listdir(self.config_dir):
                if filename.endswith('.yaml') or filename.endswith('.yml'):
                    role_name = filename.replace('.yaml', '').replace('.yml', '')
                    file_path = os.path.join(self.config_dir, filename)
                    try:
                        with open(file_path, 'r', encoding='utf-8') as f:
                            role_config = yaml.safe_load(f)
                            self.roles[role_name] = role_config
                            logger.bind(tag=TAG).info(f"加载角色配置: {role_name}")
                    except Exception as e:
                        logger.bind(tag=TAG).error(f"加载角色配置失败 {filename}: {e}")
        except Exception as e:
            logger.bind(tag=TAG).error(f"加载角色时发生错误: {e}")
    
    def get_role(self, role_name: str) -> Optional[Dict[str, Any]]:
        """获取指定角色的配置"""
        try:
            logger.bind(tag=TAG).info(f"请求获取角色配置: {role_name}")
            logger.bind(tag=TAG).info(f"当前可用角色: {list(self.roles.keys())}")
            return self.roles.get(role_name)
        except Exception as e:
            logger.bind(tag=TAG).error(f"获取角色配置时发生错误: {e}")
            return None
    
    def list_roles(self) -> Dict[str, Dict[str, Any]]:
        """列出所有可用角色"""
        return self.roles
    
    def add_role(self, role_name: str, role_config: Dict[str, Any]):
        """添加新角色"""
        try:
            self.roles[role_name] = role_config
            # 保存到文件
            file_path = os.path.join(self.config_dir, f"{role_name}.yaml")
            with open(file_path, 'w', encoding='utf-8') as f:
                yaml.dump(role_config, f, allow_unicode=True, default_flow_style=False)
            logger.bind(tag=TAG).info(f"保存角色配置到文件: {file_path}")
        except Exception as e:
            logger.bind(tag=TAG).error(f"保存角色配置失败 {role_name}: {e}")
    
    def remove_role(self, role_name: str):
        """删除角色"""
        try:
            if role_name in self.roles:
                del self.roles[role_name]
                file_path = os.path.join(self.config_dir, f"{role_name}.yaml")
                if os.path.exists(file_path):
                    os.remove(file_path)
                    logger.bind(tag=TAG).info(f"删除角色配置文件: {file_path}")
        except Exception as e:
            logger.bind(tag=TAG).error(f"删除角色配置文件失败 {role_name}: {e}")

# 全局角色管理器实例
try:
    role_manager = RoleManager()
except Exception as e:
    logger.bind(tag=TAG).error(f"初始化RoleManager失败: {e}")
    role_manager = None