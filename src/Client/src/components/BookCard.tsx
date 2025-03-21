import { Link, useNavigate } from 'react-router-dom';

import { Button, Card } from 'react-bootstrap';
import { BsCartPlus } from 'react-icons/bs';

import { useThemeHook } from '../providers/ThemeProvider';
import errorsService from '../services/errorsService';
import shoppingCartsService from '../services/shoppingCartsService';
import routes from '../common/routes';

const BookCard = (props: any) => {
  const { id, title, price, imageUrl, genre, authorName, isAvailable } = props.data;

  const [theme] = useThemeHook();
  const navigate = useNavigate();

  const addToCart = () => {
    shoppingCartsService
      .addBook({
        bookId: Number(id),
        quantity: 1
      })
      .subscribe({
        next: () => navigate(routes.cart.getRoute()),
        error: errorsService.handle
      });
  };

  return (
    <Card
      style={{ width: '18rem', height: 'auto' }}
      className={`${theme ? 'bg-light-black text-light' : 'bg-light text-black'} text-center p-0 overflow-hidden shadow mx-auto mb-4`}
    >
      <Link to={routes.bookDetails.getRoute(id)}>
        <div style={{
          background: 'white', height: '15rem', overflow: 'hidden', display: 'flex',
          justifyContent: 'center', alignItems: 'center', marginBottom: 'inherit'
        }}>
          <div style={{ width: '9rem' }}>
            <Card.Img variant='top' src={imageUrl} className='img-fluid' />
          </div>
        </div>
      </Link>
      <Card.Body>
        <Card.Title style={{ textOverflow: 'ellipsis', overflow: 'hidden', whiteSpace: 'nowrap' }}>
          {title}
        </Card.Title>
        <Card.Title style={{ textOverflow: 'ellipsis', overflow: 'hidden', whiteSpace: 'nowrap' }}>
          {authorName}
        </Card.Title>
        <Card.Title style={{ textOverflow: 'ellipsis', overflow: 'hidden', whiteSpace: 'nowrap' }}>
          {genre}
        </Card.Title>
        <Card.Title>
          <span className='h3'>${price}</span>
        </Card.Title>
        <Button
          onClick={() => addToCart()}
          disabled={!isAvailable}
          className={`${theme ? 'bg-dark-primary text-black' : 'bg-light-primary'} d-flex align-item-center m-auto border-0`}
        >
          <BsCartPlus size='1.8rem' />
          {isAvailable ? 'Add to cart' : 'Out of stock'}
        </Button>
      </Card.Body>
    </Card>
  );
};

export default BookCard;