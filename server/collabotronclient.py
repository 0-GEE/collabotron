class CollabotronClient:
    """
    This class serves as an internal representation
     of a client. It is obviously not the actual
     client itself.
    """
    def __init__(self, id, token) -> None:
        self.id = id
        self.auth_token = token
        self.updated = True

    def require_update(self):
        self.updated = False

    def is_updated(self):
        return self.updated
    
    def complete_update(self):
        self.updated = True
    
    def get_id(self):
        return self.id
    
    def get_token(self):
        return self.auth_token
    
    def __eq__(self, __value: object) -> bool:
        return self.id == __value.id
    