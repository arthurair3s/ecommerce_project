Feature: Product

Scenario: creating a product with all mandatory fields should succeed
  Given I have a valid product with the following required fields:
    | Model      | ReleaseDate | Specifications           | Price  | StockQuantity | Type       |
    | IPhone 16e | 2024-01-15  | 8GB RAM, 128GB Storage    | 699.99 |            50 | Smartphone |
  When I send a POST request to the /products endpoint with this data
  Then the response status code should be 201 Created
  And the response body should contain the created product fields

Scenario: creating a product missing a mandatory field should return an error
  Given I have an invalid product missing the required field "Model":
    | ReleaseDate | Specifications           | Price  | StockQuantity | Type       |
    | 2024-01-15  | 8GB RAM, 128GB Storage    | 699.99 |            50 | Smartphone |
  When I send a POST request to the /products endpoint with this data
  Then the response status code should be 400 Bad Request

Scenario: listing all products should succeed
  Given the following products already exist:
    | Model       | ReleaseDate | Specifications            | Price  | StockQuantity | Type       |
    | Galaxy S30  | 2024-02-20  | 12GB RAM, 256GB Storage    | 799.99 |            30 | Smartphone |
    | Pixel 9     | 2024-03-10  | 8GB RAM, 128GB Storage     | 699.99 |            40 | Smartphone |
    | Galaxy A5   | 2024-02-20  | 4GB RAM, 64GB Storage      | 499.99 |            25 | Smartphone |
  When I send a GET request to the /products endpoint
  Then the response status code should be 200 OK
  And the response body should contain the listed products

Scenario: getting a product by an existing id should return the product
  Given a product exists with the id 1
  When I send a GET request to /products/1
  Then the response status code should be 200 OK
  And the response body should contain the product with id 1

Scenario: getting a product by a nonexistent id should return an error
  Given no product exists with the id 999
  When I send a GET request to /products/999
  Then the response status code should be 404 Not Found
  And the response body should contain the error message "Product not found"

Scenario: partially updating a product should succeed
  Given a product exists with the id 2
  When I send a PATCH request to /products/2 with the following partial data:
    | Price  | StockQuantity |
    | 749.99 |            35 |
  Then the response status code should be 200 OK

Scenario: fully updating a product should save the changes
  Given a product exists with the id 3
  When I send a PUT request to /products/3 with the following updated data:
    | Price  | StockQuantity |
    | 749.99 |            35 |
  Then the response status code should be 200 OK

Scenario: deleting a product by an existing id should succeed
  Given a product exists with the id 3
  When I send a DELETE request to /products/3
  Then the response status code should be 204 No Content
