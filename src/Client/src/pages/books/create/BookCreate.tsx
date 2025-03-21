import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

import 'react-lightbox-component/build/css/index.css';
import { Button, Col, Container, Form, Row, Spinner } from 'react-bootstrap';

import { useThemeHook } from '../../../providers/ThemeProvider';
import booksService from '../../../services/booksService';
import errorsService from '../../../services/errorsService';
import routes from '../../../common/routes';
import { Genre } from '../../../models/Genre';

const BookCreate = (): JSX.Element => {
  const navigate = useNavigate();
  const [theme] = useThemeHook();
  const [loading, setLoading] = useState(false);
  const [title, setTitle] = useState<string>('');
  const [price, setPrice] = useState<number>(1);
  const [quantity, setQuantity] = useState<number>(1);
  const [imageUrl, setImageUrl] = useState<string>('');
  const [description, setDescription] = useState<string>('');
  const [genre, setGenre] = useState<number>(Genre.Horror.value);
  const [author, setAuthor] = useState<string>('');

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (title && price && quantity && imageUrl && description && genre && author) {
      setLoading(true);

      booksService
        .create({
          title,
          price,
          quantity,
          imageUrl,
          description,
          genre,
          author
        })
        .subscribe({
          next: res => {
            const id = Number(res.data);
            setLoading(false);
            navigate(routes.bookDetails.getRoute(id));
          },
          error: err => {
            setLoading(false);
            errorsService.handle(err);
          }
        });
    }
  };

  return (
    <Container className='py-5 mt-5'>
      <Row className='justify-content-center mt-5'>
        <Col xs={11} sm={10} md={8} lg={4}
             className={`p-4 rounded ${theme ? 'text-light bg-dark' : 'text-black bg-light'}`}>
          <h1 className={`text-center border-bottom pb-3 ${theme ? 'text-dark-primary' : 'text-light-primary'}`}>
            Create Book
          </h1>
          <Form onSubmit={(e) => handleSubmit(e)}>
            <Form.Group className='mb-3'>
              <Form.Label>Title</Form.Label>
              <Form.Control name='title'
                            type='text'
                            placeholder='Title'
                            required
                            value={title}
                            onChange={e => setTitle(e.target.value)} />
            </Form.Group>
            <Row>
              <Form.Group className='mb-3 col-lg-6'>
                <Form.Label>Price</Form.Label>
                <Form.Control name='price'
                              type='number'
                              placeholder='Price'
                              required
                              value={price}
                              onChange={e => setPrice(Number(e.target.value))} />
              </Form.Group>
              <Form.Group className='mb-3 col-lg-6'>
                <Form.Label>Quantity</Form.Label>
                <Form.Control name='quantity'
                              type='number'
                              placeholder='Quantity'
                              required
                              value={quantity}
                              onChange={e => setQuantity(Number(e.target.value))} />
              </Form.Group>
            </Row>
            <Form.Group className='mb-3'>
              <Form.Label>Image URL</Form.Label>
              <Form.Control name='imageUrl'
                            type='url'
                            placeholder='Image URL'
                            required
                            value={imageUrl}
                            onChange={e => setImageUrl(e.target.value)} />
            </Form.Group>
            <Form.Group className='mb-3'>
              <Form.Label>Description</Form.Label>
              <Form.Control name='description'
                            as='textarea'
                            type='text'
                            placeholder='Description'
                            required
                            value={description}
                            onChange={e => setDescription(e.target.value)} />
            </Form.Group>
            <Form.Group className='mb-3'>
              <Form.Label>Genre</Form.Label>
              <Form.Select name='genre'
                           placeholder='Genre'
                           required
                           value={genre}
                           onChange={e => setGenre(Number(e.target.value))}>
                {Genre.values.map((genre, index) => {
                  return <option key={index} value={genre.value}>{genre.name}</option>;
                })}
              </Form.Select>
            </Form.Group>
            <Form.Group className='mb-3'>
              <Form.Label>Author</Form.Label>
              <Form.Control name='author'
                            type='text'
                            placeholder='Author'
                            required
                            value={author}
                            onChange={e => setAuthor(e.target.value)} />
            </Form.Group>
            <Button
              type='submit'
              className={`${theme ? 'bg-dark-primary text-black' : 'bg-light-primary'} m-auto d-block`}
              disabled={loading}
              style={{ border: 0 }}
            >
              {loading ?
                <>
                  <Spinner
                    as='span'
                    animation='grow'
                    size='sm'
                    role='status'
                    aria-hidden='true'
                  />
                  &nbsp;Loading...
                </> : 'Create'
              }
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default BookCreate;