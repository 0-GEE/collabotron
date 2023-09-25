class CollabotronClient:
    def __init__(self, id, token) -> None:
        self.id = id
        self.auth_token = token
        self.updated = True

    def require_update(self):
        self.updated = False

    def is_updated(self):
        return self.updated
    
    def get_id(self):
        return self.id
    
    def get_token(self):
        return self.auth_token
    