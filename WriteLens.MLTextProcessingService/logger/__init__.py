import logging

logging.getLogger("httpx").setLevel(logging.INFO)
logging.getLogger("httpcore").setLevel(logging.INFO)

logging.basicConfig(level=logging.DEBUG,
                    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
                    handlers=[logging.StreamHandler(), 
                              logging.FileHandler('app.log')])

logger = logging.getLogger(__name__)


def get_logger(name: str = __name__) -> logging.Logger:
    logging.basicConfig(level=logging.DEBUG,
                    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
                    handlers=[logging.StreamHandler(), 
                              logging.FileHandler('app.log')])

    return logging.getLogger(name) 